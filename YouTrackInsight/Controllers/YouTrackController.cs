﻿using System;
using System.Net;
using Bakhoo;
using Bakhoo.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using YouTrackInsight.Domain;
using YouTrackInsight.Entity;
using YouTrackInsight.Services;

namespace YouTrackInsight.Controllers;

[ApiController]
[Route("api/[controller]")]
public class YouTrackController : ControllerBase
{
    private readonly IBakhooJobWindow _jobWindow;
    private readonly YTIssueService _issueService;
    private readonly YTIssueImportService _issueImportService;
    private readonly YouTrackInsightHubClients _hubClients;

    public YouTrackController(
        YTIssueService issueService,
        IBakhooJobWindow jobWindow,
        YTIssueImportService issueImportService,
        YouTrackInsightHubClients hubClients)
    {
        _issueService = issueService;
        _jobWindow = jobWindow;
        _issueImportService = issueImportService;
        _hubClients = hubClients;
    }

    [HttpGet("issue-network")]
    public async Task<YTIssueNetworkModel> GetIssueNetwork([FromQuery] YTSearchOptions options)
    {
        var issues = await _issueService.GetIssueNetwork(options);
        return issues;
    }

    [HttpGet("issue-import")]
    public IAsyncEnumerable<BakhooJob> GetIssueImportTasks([FromQuery] IEnumerable<Guid> id)
        => _jobWindow.GetJobsAsync();

    public class SubmitIssueImportRequest
    {
        public Guid Id { get; set; }
    }

    [HttpPut("issue-import")]
    public async Task<ActionResult> SubmitIssueImport([FromBody] SubmitIssueImportRequest request, CancellationToken ct)
    {
        try
        {
            await _jobWindow.SubmitJobAsync(request.Id, new YTIssueImportJob { }, ct);
        }
        catch (ArgumentException e)
        {
            return Problem(e.Message, statusCode: (int)HttpStatusCode.BadRequest);
        }
        catch (InvalidOperationException e)
        {
            return Problem(e.Message);
        }

        await _hubClients.NotifyIssueImportJobUpdatedAsync(request.Id, ct);

        return Ok();
    }

    public class CancelIssueImportRequest
    {
        public Guid Id { get; set; }
    }

    [HttpDelete("issue-import")]
    public async Task<ActionResult> CancelIssueImport([FromBody] CancelIssueImportRequest request, CancellationToken ct)
    {
        try
        {
            await _jobWindow.CancelJobAsync(request.Id, ct);
        }
        catch (ArgumentException e)
        {
            return Problem(e.Message, statusCode: (int)HttpStatusCode.BadRequest);
        }
        catch (InvalidOperationException e)
        {
            return Problem(e.Message);
        }

        await _hubClients.NotifyIssueImportJobUpdatedAsync(request.Id, ct);

        return Ok();
    }
}

