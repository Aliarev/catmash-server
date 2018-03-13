using Catmash.Dto;
using Catmash.Model;
using Catmash.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Catmash.Service
{
    public class CatService
    {
        private CatmashContext _context { get; set; }


        public CatService(CatmashContext context)
        {
            _context = context;
        }


        private CatDto ToDto(Cat model)
        {
            CatDto dto = new CatDto();

            if (model == null)
                return dto;

            dto.Id = model.Id;
            dto.PictureUrl = model.PictureUrl;
            dto.VoteDown = model.VoteDown;
            dto.VoteUp = model.VoteUp;

            return dto;
        }

        private Cat ToModel(CatDto dto, Cat model)
        {
            if (model == null)
                model = new Cat();

            if (dto == null)
                return null;

            model.PictureUrl = dto.PictureUrl;
            model.VoteDown = dto.VoteDown;
            model.VoteUp = dto.VoteUp;

            return model;
        }

        private async Task<bool> InitIfNecessary()
        {
            // Initialisation à partir du fichier "cats.json"
            if (_context.Cats.Count() == 0)
            {
                try
                {
                    using (StreamReader sr = new StreamReader("./cats.json"))
                    {
                        string json = sr.ReadToEnd();
                        dynamic file = JsonConvert.DeserializeObject(json);
                        foreach (var cat in file.images)
                        {
                            await Save(new CatDto
                            {
                                PictureUrl = cat.url
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return true;
        }


        public async Task<List<CatDto>> GetAllAsync()
        {
            await InitIfNecessary();

            return await _context.Cats.Select(c => ToDto(c)).ToListAsync();
        }

        public async Task<bool> Save(CatDto dto)
        {
            if (dto == null)
                return false;

            Cat model = await _context.Cats.FindAsync(dto.Id);

            bool isNew = model == null;

            model = ToModel(dto, model);

            if (isNew)
                _context.Cats.Add(model);
            else
                _context.Cats.Update(model);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Vote(VoteDto vote)
        {
            await InitIfNecessary();

            if (vote == null)
                return false;

            Cat catDown = await _context.Cats.FirstOrDefaultAsync(c => c.Id == vote.IdDown);
            if (catDown == null)
                return false;

            Cat catUp = await _context.Cats.FirstOrDefaultAsync(c => c.Id == vote.IdUp);
            if (catUp == null)
                return false;

            catDown.VoteDown++;
            catUp.VoteUp++;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
