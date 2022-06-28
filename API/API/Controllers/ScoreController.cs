using API.Data;
using API.Models.DatabaseModels;
using API.Models.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [EnableCors("Allow CORS")]
    [Route("api/score")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ScoreController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Route("class/{courseClassId}")]
        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<List<ReturnedScore>>> GetScoreByClass(string courseClassId)
        {
            List<Score> scoreList = (from w in _context.Score
                                     where w.CourseClassroomId == courseClassId
                                     select w).ToList();
            List<double> totalScoreList = new List<double>();
            foreach (var score in scoreList)
            {
                totalScoreList.Add(score.CalScore());
            }

            List<ReturnedScore> returnedScore = new List<ReturnedScore>();
            for (int i = 0; i < scoreList.Count; i++)
            {
                User userInformation = await _context.User.FindAsync(scoreList[i].UserId);
                ReturnedScore temp = new ReturnedScore
                {
                    Student = userInformation.Name,
                    Score = scoreList[i],
                    TotalScore = scoreList[i].CalScore()
                };
                returnedScore.Add(temp);
            }

            return Ok(returnedScore);
        }
        [Route("student/{userInformationId}")]
        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<List<ReturnedScoreOfStudent>>> GetAllScore(string userInformationId)
        {
            var scoreList = (from w in _context.Score
                             where w.UserId == userInformationId
                             select w).ToList();
            List<double> totalScoreList = new List<double>();
            foreach (var score in scoreList)
            {
                totalScoreList.Add(score.CalScore());
            }
            List<ReturnedScoreOfStudent> returnedScore = new List<ReturnedScoreOfStudent>();
            for (int i = 0; i < scoreList.Count; i++)
            {
                CourseClassroom courseClassroom = await _context.CourseClassroom.FindAsync(scoreList[i].CourseClassroomId);
                ReturnedScoreOfStudent temp = new ReturnedScoreOfStudent
                {
                    CourseClassroom = courseClassroom,
                    Score = scoreList[i],
                    TotalScore = scoreList[i].CalScore()
                };
                returnedScore.Add(temp);
            }

            return Ok(returnedScore);
        }
        [Route("{userInformationId}/{courseClassId}")]
        [HttpPut]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<List<ReturnedScore>>> Update(string userInformationId, string courseClassId, UpdateScoreDto request)
        {
            Score findingScore = await _context.Score
                .Where(w => w.UserId == userInformationId && w.CourseClassroomId == courseClassId)
                .FirstAsync();
            findingScore.ExcerciseScore = request.ExcerciseScore;
            findingScore.MidTermScore = request.MidTermScore;
            findingScore.FinalTermScore = request.FinalTermScore;
            await _context.SaveChangesAsync();
            return await GetScoreByClass(courseClassId);
        }
    }
}