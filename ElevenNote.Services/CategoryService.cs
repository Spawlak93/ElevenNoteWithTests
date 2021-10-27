using ElevenNote.Data;
using ElevenNote.Models;
using ElevenNote.Models.CategoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class CategoryService
    {
        private readonly Guid _userId;

        public CategoryService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateCategory(CategoryCreate model)
        {
            var entity =
                new Category
                {
                    Name = model.Name,
                    OwnerId = _userId
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Categories.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }


        public IEnumerable<CategoryListItem> GetAllCategories()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Categories
                        .Where(e => e.OwnerId == _userId)
                        .Select(
                            e =>
                                new CategoryListItem
                                {
                                    CategoryId = e.Id,
                                    Name = e.Name,
                                    NumberOfNotesInCategory = e.Notes.Count
                                }
                        );

                return query.ToArray();
            }
        }

        public CategoryDetail GetCategoryById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Categories
                        .SingleOrDefault(e => e.Id == id && e.OwnerId == _userId);

                return
                    new CategoryDetail
                    {
                        CategoryId = entity.Id,
                        Name = entity.Name,
                        Notes = entity.Notes.Select(e =>
                                                        new NoteListItem
                                                        {
                                                            NoteId = e.NoteId,
                                                            Title = e.Title,
                                                            CategoryName = e.Category.Name,
                                                            CreatedUtc = e.CreatedUtc
                                                        }).ToList()
                    };
            }
        }

        public bool UpdateCategory(CategoryEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Categories
                        .Single(e => e.Id == model.CategoryId && e.OwnerId == _userId);

                entity.Name = model.Name;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteCategory(int categoryId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Categories
                        .Single(e => e.Id == categoryId && e.OwnerId == _userId);

                ctx.Categories.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }

    }
}
