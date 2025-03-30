using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadsOfRussiaAPI.Controllers.Model;
using RoadsOfRussiaAPI.DbModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace RoadsOfRussiaAPI.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly ApplicationDbContext database;

        public DocumentsController(ApplicationDbContext database)
        {
            this.database = database;
        }

        [HttpPost("SignIn")]
        public IActionResult Authorization([FromBody] AuthorizationModel request)
        {
            if (request.name == "admin" && request.password == "admin")
            {
                var claims = new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Name, request.name),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("HDwuiahuidsja&6728931e18nhx81hdn8x1h8sjf4e8w94fds16165u1kjyt"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "First",
                    audience: "Employee",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                    );

                return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return StatusCode(403, new ErrorModel() { timestamp = DateTime.UtcNow.Ticks, message = "Неверные авторизационные данные", errorCode = 1566 });
        }

        [Authorize]
        [HttpGet("Documents")]
        public async Task<IActionResult> GetDocuments()
        {
            try
            {
                var documents = await (from document in database.Document
                                 join documentCategory in database.DocumentCategory on document.IDDocumentCategory equals documentCategory.IDDocumentCategory
                                 select new
                                 { 
                                    document.IDDocument,
                                    document.Title,
                                    document.DateCreate,
                                    document.DateUpdate,
                                    Category = documentCategory.Title,
                                 }).ToListAsync();

                if (documents.Count != 0)
                {
                    var documentList = new List<DocumentsModel>();

                    foreach (var document in documents)
                    {
                        var documentTemplate = new DocumentsModel();

                        documentTemplate.id = document.IDDocument;
                        documentTemplate.title = document.Title;
                        documentTemplate.date_created = document.DateCreate;
                        documentTemplate.date_updated = document.DateUpdate;
                        documentTemplate.category = document.Category;
                        documentTemplate.has_comments = database.Comment.FirstOrDefault(x=>x.IDDocument == document.IDDocument) != null ? true : false;

                        documentList.Add(documentTemplate);
                    }

                    return Ok(documentList);
                }
                else
                {
                    return StatusCode(404, new ErrorModel() { timestamp = DateTime.UtcNow.Ticks, message = "Не найденно данных", errorCode = 1568 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ErrorModel() { timestamp = DateTime.UtcNow.Ticks, message = "Ошибка подключения к базе данных", errorCode = 1567 });
            }
        }

        [Authorize]
        [HttpGet("Documents/{documentId}/Comments")]
        public async Task<IActionResult> GetComment(int documentId)
        {
            try
            {
                var comments = await (from comment in database.Comment
                                      join document in database.Document on comment.IDDocument equals document.IDDocument
                                      join employee in database.Employee on comment.IDAuthor equals employee.IDEmployee
                                      join post in database.Post on employee.IDPost equals post.IDPost
                                      where (comment.IDDocument == documentId)
                                      select new
                                      {
                                          comment.IDComment,
                                          comment.IDDocument,
                                          comment.Text,
                                          comment.DateCreate,
                                          comment.DateUpdate,
                                          Author = employee.Surname + " " + employee.Name,
                                          Post = post.Title
                                      }).ToListAsync();

                if (comments.Count != 0)
                {
                    var commentsList = new List<CommentModel>();

                    foreach (var comment in comments)
                    {
                        var commentTemplate = new CommentModel();

                        commentTemplate.id = comment.IDComment;
                        commentTemplate.document_id = comment.IDDocument;
                        commentTemplate.text = comment.Text;
                        commentTemplate.date_created = comment.DateCreate;
                        commentTemplate.date_updated = comment.DateUpdate;
                        commentTemplate.author = new CommentModel.authorClass() { name = comment.Author, position = comment.Post };

                        commentsList.Add(commentTemplate);
                    }

                    return Ok(commentsList);
                }
                else
                {
                    return StatusCode(404, new ErrorModel() { timestamp = DateTime.UtcNow.Ticks, message = "Не найденно данных", errorCode = 1568 });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ErrorModel() { timestamp = DateTime.UtcNow.Ticks, message = "Ошибка подключения к базе данных", errorCode = 1567 });
            }
        }

        [Authorize]
        [HttpPost("Documents/{documentId}/Comment")]
        public async Task<IActionResult> SetComment(int documentId, [FromBody] CommentContextModel request)
        {
            try
            {
                var comment = database.Comment.Where(x=>x.IDDocument == documentId && x.IDAuthor == request.autor_id).FirstOrDefault();

                if (comment == null)
                {
                    var newComment = new Comment();

                    newComment.IDDocument = documentId;
                    newComment.Text = request.text;
                    newComment.DateCreate = DateTime.UtcNow;
                    newComment.DateUpdate = DateTime.UtcNow;
                    newComment.IDAuthor = request.autor_id;

                    database.Comment.Add(newComment);
                    await database.SaveChangesAsync();
                }
                else
                { 
                    comment.Text = request.text;
                    comment.DateUpdate = DateTime.UtcNow;

                    await database.SaveChangesAsync();
                }

                return Ok(new { message = "Данные сохранены"});
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ErrorModel() { timestamp = DateTime.UtcNow.Ticks, message = "Ошибка подключения к базе данных", errorCode = 1567 });
            }
        }
    }
}
