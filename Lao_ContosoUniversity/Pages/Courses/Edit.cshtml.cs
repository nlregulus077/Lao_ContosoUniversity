﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lao_ContosoUniversity.Models;

namespace Lao_ContosoUniversity.Pages.Courses
{
    public class EditModel : DepartmentNamePageModel
    {
        private readonly Lao_ContosoUniversity.Models.SchoolContext _context;

        public EditModel(Lao_ContosoUniversity.Models.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await _context.Course
                .Include(c => c.Department).FirstOrDefaultAsync(m => m.CourseID == id);

            if (Course == null)
            {
                return NotFound();
            }

            PopulateDepartmentDropDownList(_context, Course.DepartmentID);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var courseToUpdate = await _context.Course.FindAsync(id);

            if (await TryUpdateModelAsync<Course>( 
                    courseToUpdate,
                    "course",
                       c => c.Credits, c => c.DepartmentID, c => c.Title)) 
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PopulateDepartmentDropDownList(_context, courseToUpdate.DepartmentID);
            return Page();
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseID == id);
        }
    }
}
