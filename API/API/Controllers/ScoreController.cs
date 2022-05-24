using API.Data;
using API.Models.DatabaseModels;
using API.Models.DtoModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [EnableCors("Cau Khong")]
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
        public async Task<ActionResult<List<ReturnedScore>>> GetScoreByClass(int CourseClassId)
        {
            List<Score> scoreList = (from w in _context.Score
                where w.CourseClassroomId == CourseClassId
                select w).ToList();
            List<double> totalScoreList = new List<double>();
            foreach (var score in scoreList)
            {
                totalScoreList.Add(score.calScore());
            }

            List<ReturnedScore> returnedScore = new List<ReturnedScore>();
            for (int i = 0; i < scoreList.Count; i++)
            {
                UserInformation userInformation = await _context.UsersInformation.FindAsync(scoreList[i].UserInformationId);
                ReturnedScore temp = new ReturnedScore
                {
                    information = userInformation.Name,
                    score = scoreList[i],
                    totalScore = scoreList[i].calScore()
                };
                returnedScore.Add(temp);
            }

            return Ok(returnedScore);
        }
        [Route("student/{userInformationId}")]
        [HttpGet]
        public async Task<ActionResult<List<ReturnedScoreOfStudent>>> GetAllScore(string UserInformationId)
        {
            var scoreList = (from w in _context.Score
                where w.UserInformationId == UserInformationId
                select w).ToList();
            List<double> totalScoreList = new List<double>();
            foreach (var score in scoreList)
            {
                totalScoreList.Add(score.calScore());
            }
            List<ReturnedScoreOfStudent> returnedScore = new List<ReturnedScoreOfStudent>();
            for (int i = 0; i < scoreList.Count; i++)
            {
                CourseClassroom courseClassroom = await _context.CoursesClassroom.FindAsync(scoreList[i].CourseClassroomId);
                ReturnedScoreOfStudent temp = new ReturnedScoreOfStudent
                {
                     CourseClassroom = courseClassroom,
                     score = scoreList[i],
                     totalScore = scoreList[i].calScore()
                };
                returnedScore.Add(temp);
            }

            return Ok(returnedScore);
        }
        [Route("{userInformationId}/{courseClassId}")]
        [HttpPut]
        public async Task<ActionResult<List<ReturnedScore>>> Update(string userInformationId, int CourseClassId, UpdateScoreDto request)
        {
            Score findingScore = await _context.Score
                .Where(w => w.UserInformationId == userInformationId && w.CourseClassroomId == CourseClassId)
                .FirstAsync();
            findingScore.excerciseScore = request.excerciseScore;
            findingScore.midTermScore = request.midTermScore;
            findingScore.finalTermScore = request.finalTermScore;
            await _context.SaveChangesAsync();
            return await GetScoreByClass(CourseClassId);
        }
    }
}
