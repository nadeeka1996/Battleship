using Moq;
using Battleship.Api.Endpoints;
using Battleship.Application.Interfaces.Services;
using Battleship.Application.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Battleship.Application.Common;
using Battleship.Application.Responses;

namespace Battleship.Tests;

/// <summary>
/// Unit tests for the GameEndpoints API endpoints.
/// Covers Create, Get, and PostShot scenarios including success and error cases.
/// </summary>
public class GameEndpointsTests
{
    /// <summary>
    /// Tests that Create returns Created when a game is successfully created.
    /// </summary>
    [Fact]
    public async Task Create_ReturnsCreated_WhenGameCreated()
    {

        var mockService = new Mock<IGameService>();
        var gamePostResponse = new GamePostResponse(Guid.NewGuid());
        var result = new Result<GamePostResponse>(true, gamePostResponse);

        mockService
            .Setup(s => s.CreateAsync())
            .ReturnsAsync(result);

        var actionResult = await GameEndpoints.Create(mockService.Object);

        var createdResult = Assert.IsType<Created<GamePostResponse>>(actionResult);
        Assert.Equal(gamePostResponse, createdResult.Value);
    }

    /// <summary>
    /// Tests that Get returns Ok when the game exists.
    /// </summary>
    [Fact]
    public async Task Get_ReturnsOk_WhenGameExists()
    {
        var mockService = new Mock<IGameService>();

        var id = Guid.NewGuid();
        var shots = new List<ShotResponse>();
        var ships = new List<ShipResponse>();
        bool isGameOver = false;

        var gameStateResponse = new GameStateGetResponse(id, shots, ships, isGameOver);
        var result = new Result<GameStateGetResponse>(true, gameStateResponse);

        mockService
            .Setup(s => s.GetStateAsync(It.IsAny<Guid>()))
            .ReturnsAsync(result);

        var actionResult = await GameEndpoints.Get(Guid.NewGuid(), mockService.Object);

        var okResult = Assert.IsType<Ok<GameStateGetResponse>>(actionResult);
        Assert.Equal(gameStateResponse, okResult.Value);
    }

    /// <summary>
    /// Tests that Create returns BadRequest when game creation fails.
    /// </summary>
    [Fact]
    public async Task Create_ReturnsBadRequest_WhenGameCreationFails()
    {
        var mockService = new Mock<IGameService>();
        var error = Result<GamePostResponse>.Failure("creation failed");

        mockService
            .Setup(s => s.CreateAsync())
            .ReturnsAsync(error);

        var result = await GameEndpoints.Create(mockService.Object);

        var badRequest = Assert.IsType<BadRequest<Envelope<string>>>(result);
        Assert.Contains("creation failed", badRequest.Value.Errors);
    }

    /// <summary>
    /// Tests that Get returns NotFound when the game does not exist.
    /// </summary>
    [Fact]
    public async Task Get_ReturnsNotFound_WhenGameDoesNotExist()
    {
        var mockService = new Mock<IGameService>();
        var errorResult = Result<GameStateGetResponse>.Failure("not found");

        mockService.Setup(s => s.GetStateAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(errorResult);

        var result = await GameEndpoints.Get(Guid.NewGuid(), mockService.Object);

        var notFound = Assert.IsType<NotFound<Envelope<string>>>(result);
        Assert.Contains("not found", notFound.Value.Errors);
    }

    /// <summary>
    /// Tests that PostShot returns Ok when the shot is successful.
    /// </summary>
    [Fact]
    public async Task PostShot_ReturnsOk_WhenShotIsSuccessful()
    {
        var mockService = new Mock<IGameService>();
        var gameId = Guid.NewGuid();
        var testCordinate = "A1";
        var request = new GameShotPostRequest(testCordinate);

        var response = new GameShotPostResponse(true, false, false); 
        var result = Result<GameShotPostResponse>.Success(response);

        mockService
            .Setup(s => s.PostShotAsync(gameId, It.IsAny<GameShotPostRequest>()))
            .ReturnsAsync(result);

        var resultActual = await GameEndpoints.PostShot(gameId, request, mockService.Object);

        var okResult = Assert.IsType<Ok<GameShotPostResponse>>(resultActual);
        Assert.True(okResult?.Value?.Hit);
    }

    /// <summary>
    /// Tests that PostShot returns NotFound when the game is not found.
    /// </summary>
    [Fact]
    public async Task PostShot_ReturnsNotFound_WhenGameNotFound()
    {
        var mockService = new Mock<IGameService>();
        var gameId = Guid.NewGuid();
        var testCordinate = "A1";
        var request = new GameShotPostRequest(testCordinate);

        var resultEnvelope = Result<GameShotPostResponse>.Failure("not found");

        mockService
            .Setup(s => s.PostShotAsync(gameId, It.IsAny<GameShotPostRequest>()))
            .ReturnsAsync(resultEnvelope); 

        var result = await GameEndpoints.PostShot(gameId, request, mockService.Object);

        var notFound = Assert.IsType<NotFound<Envelope<string>>>(result);
        Assert.Contains("not found", notFound.Value.Errors);
    }

    /// <summary>
    /// Tests that PostShot returns BadRequest when the shot is invalid.
    /// </summary>
    [Fact]
    public async Task PostShot_ReturnsBadRequest_WhenShotFails()
    {
        var mockService = new Mock<IGameService>();
        var gameId = Guid.NewGuid();
        var testCordinate = "A1";
        var request = new GameShotPostRequest(testCordinate);

        var resultFailure = Result<GameShotPostResponse>.Failure("invalid shot");

        mockService
            .Setup(s => s.PostShotAsync(gameId, It.IsAny<GameShotPostRequest>()))
            .ReturnsAsync(resultFailure); 

        var result = await GameEndpoints.PostShot(gameId, request, mockService.Object);

        var badRequest = Assert.IsType<BadRequest<Envelope<string>>>(result);
        Assert.Contains("invalid shot", badRequest.Value.Errors);
    }
}
