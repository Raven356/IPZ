﻿using AutoMapper;
using BlogMVC.BLL.Models;
using BlogMVC.DAL.Models;
using BlogMVC.DAL.Repository;
using BlogMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.BLL.Services.TagsService
{
    public class TagsService : ITagsService
    {
        private readonly IRepository<Tags> _tagsRepository;
        private readonly ITagsMongoRepository _tagsMongoRepository;
        private readonly IRepository<TagToBlogPost> _tagsToBlogPostRepository;
        private readonly ITagToBlogPostRepository _tagToBlogPostRepository;
        private readonly IRepository<BlogPost> _blogPostRepository;
        private readonly IBlogPostMongoRepository _blogPostMongoRepository;
        private readonly IMapper _mapper;

        public TagsService(IRepository<Tags> tagsRepository
            , IRepository<TagToBlogPost> tagsToBlogPostRepository
            , IRepository<BlogPost> blogPostRepository, IMapper mapper, ITagsMongoRepository tagsMongoRepository, ITagToBlogPostRepository tagToBlogPostRepository, IBlogPostMongoRepository blogPostMongoRepository)
        {
            _tagsRepository = tagsRepository;
            _tagsToBlogPostRepository = tagsToBlogPostRepository;
            _blogPostRepository = blogPostRepository;
            _mapper = mapper;
            _tagsMongoRepository = tagsMongoRepository;
            _tagToBlogPostRepository = tagToBlogPostRepository;
            _blogPostMongoRepository = blogPostMongoRepository;
        }

        public async Task Create(IEnumerable<string> tags, int blogId)
        {
            if (tags == null)
            {
                return;
            }
            var existing = _tagsRepository.GetAll();
            foreach (var tag in tags)
            {
                Tags newTag = new Tags();
                if (!existing.Select(t => t.Name).Contains(tag))
                {
                    newTag = await _tagsRepository.Add(new Tags { Name = tag });
                }
                else
                {
                    newTag = existing.Where(t => t.Name == tag).First();
                }
                await _tagsToBlogPostRepository
                    .Add(new TagToBlogPost { BlogPostId = blogId, TagId =  newTag.Id });
            }
        }

        public void CreateMongo(IEnumerable<string> tags, string blogId)
        {
            if (tags == null)
            {
                return;
            }
            var existing = _tagsMongoRepository.GetAll();
            foreach (var tag in tags)
            {
                TagsMongo newTag = new TagsMongo();
                if (!existing.Select(t => t.Name).Contains(tag))
                {
                    newTag = _tagsMongoRepository.Add(new TagsMongo { Name = tag });
                }
                else
                {
                    newTag = existing.Where(t => t.Name == tag).First();
                }
                _tagToBlogPostRepository.Add(new TagToBlogPostMongo { BlogPostId = blogId, TagId = newTag.Id });
            }
        }

        public void UpdateMongo(IEnumerable<string> tags, string blogId)
        {
            if (tags == null || tags.Count() <= 0)
            {
                var deleteId = _tagToBlogPostRepository.GetAll()
                    .Where(t => t.BlogPostId == blogId)
                    .Select(t => t.TagId).ToList();
                foreach (var id in deleteId)
                {
                    _tagsMongoRepository.Delete(id);
                }
                return;
            }

            var existing = _tagsMongoRepository.GetAll();
            var tagToBlog = _tagToBlogPostRepository.GetAll();
            var existingTags = _tagToBlogPostRepository.GetAll().Where(t => t.BlogPostId == blogId);

            List<string> addedId = new List<string>();
            foreach (var tag in tags)
            {
                TagsMongo newTag = new TagsMongo();
                if (!existing.Select(t => t.Name).Contains(tag))
                {
                    newTag = _tagsMongoRepository.Add(new TagsMongo { Name = tag });
                }
                else
                {
                    newTag = existing.Where(t => t.Name == tag).First();
                }
                if (tagToBlog.Where(t => t.TagId == newTag.Id && t.BlogPostId == blogId).Count() == 0)
                {
                    _tagToBlogPostRepository.Add(new TagToBlogPostMongo { BlogPostId = blogId, TagId = newTag.Id });
                }
                addedId.Add(newTag.Id);

            }
            var deleteTags = existingTags.Where(t => !addedId.Contains(t.TagId)).ToList();
            foreach (var tag in deleteTags)
            {
                _tagToBlogPostRepository.Delete(tag.Id);
            }

        }

        public async Task Update(IEnumerable<string> tags, int blogId)
        {
            if (tags == null || tags.Count() <= 0)
            {
                var deleteId = await _tagsToBlogPostRepository.GetAll()
                    .Where(t => t.BlogPostId == blogId)
                    .Select(t => t.TagId).ToListAsync();
                foreach (var id in deleteId)
                {
                    await _tagsRepository.Delete(id);
                }
                return;
            }

            var existing = _tagsRepository.GetAll();
            var tagToBlog = _tagsToBlogPostRepository.GetAll();
            var existingTags = _tagsToBlogPostRepository.GetAll().Where(t => t.BlogPostId == blogId);

            List<int> addedId = new List<int>();
            foreach (var tag in tags)
            {
                Tags newTag = new Tags();
                if (!existing.Select(t => t.Name).Contains(tag))
                {
                    newTag = await _tagsRepository.Add(new Tags { Name = tag });
                }
                else
                {
                    newTag = existing.Where(t => t.Name == tag).First();
                }
                if (tagToBlog.Where(t => t.TagId == newTag.Id && t.BlogPostId == blogId).Count() == 0)
                {
                    await _tagsToBlogPostRepository
                        .Add(new TagToBlogPost { BlogPostId = blogId, TagId = newTag.Id });
                }
                addedId.Add(newTag.Id);

            }
            var deleteTags = existingTags.Where(t => !addedId.Contains(t.TagId)).ToList();
            foreach (var tag in deleteTags)
            {
                await _tagsToBlogPostRepository.Delete(tag.Id);
            }

        }

        public IEnumerable<TagsDTOMongo> GetByBlogPostIdMongo(string? id)
        {
            var tagToBlogPost = _tagToBlogPostRepository.GetAll().Where(t => t.BlogPostId == id);
            var tags = _tagsMongoRepository.GetAll()
                .Where(t => tagToBlogPost.Select(tb => tb.TagId).Contains(t.Id));
            return _mapper.Map<IEnumerable<TagsDTOMongo>>(tags);
        }

        public async Task<IEnumerable<TagsDTO>> GetByBlogPostId(int? id)
        {
            var tagToBlogPost = _tagsToBlogPostRepository.GetAll().Where(t => t.BlogPostId == id);
            var tags = _tagsRepository.GetAll()
                .Where(t => tagToBlogPost.Select(tb => tb.TagId).Contains(t.Id));
            return _mapper.Map<IEnumerable<TagsDTO>>(tags);
        }

        public IEnumerable<BlogPostDTOMongo> GetByTagMongo(string tag)
        {
            var tags = _tagsMongoRepository.GetAll()
                .Where(t => t.Name == tag).First();
            var tagToRepo = _tagToBlogPostRepository
                .GetAll().Where(t => t.TagId == tags.Id);
            var blogs = _blogPostMongoRepository.GetAll()
                .Where(b => tagToRepo.Select(t => t.BlogPostId).Contains(b.Id));
            var result = new List<BlogPostDTOMongo>();
            blogs.OrderBy(b => b.Title);
            foreach (var item in blogs)
            {
                result.Add(_mapper.Map<BlogPostDTOMongo>(item));
            }
            return result;
        }

        public async Task<IEnumerable<BlogPostDTO>> GetByTag(string tag)
        {
            var tags = await _tagsRepository.GetAll()
                .Where(t => t.Name == tag).FirstAsync();
            var tagToRepo = _tagsToBlogPostRepository
                .GetAll().Where(t => t.TagId == tags.Id);
            var blogs = _blogPostRepository.GetAll()
                .Where(b => tagToRepo.Select(t => t.BlogPostId).Contains(b.Id));
            var result = new List<BlogPostDTO>();
            await blogs.OrderBy(b => b.Title)
                .ForEachAsync(b => result.Add(_mapper.Map<BlogPostDTO>(b)));
            return result;
        }
    }
}
