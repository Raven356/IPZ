using AutoMapper;
using BlogMVC.BLL.Models;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;

namespace BlogMVC.BLL.Services.ControllersService
{
    public class CommentsService : ICommentsService
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IMapper _mapper;
        private readonly ICommentMongoRepository _commentMongoRepository;

        public CommentsService(IRepository<Comment> commentRepository, IMapper mapper, ICommentMongoRepository commentMongoRepository)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _commentMongoRepository = commentMongoRepository;
        }

        public async Task AddNew(CommentDTO newComment)
        {
            await _commentRepository.Add(_mapper.Map<Comment>(newComment));
        }

        public void AddNewMongo(CommentDTOMongo newComment)
        {
            _commentMongoRepository.Add(_mapper.Map<CommentMongo>(newComment));
        }
    }
}
