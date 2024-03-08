using Employee_Management.Data;
using Employee_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

[Authorize]
public class EmployeeController : Controller
{
    private readonly ApplicationDbContext _context;

    public EmployeeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        try
        {
            var employees = _context.Employees
                .OrderByDescending(e => e.Id)
                .Take(50)
                .ToList();
            return View(employees);
        }
        catch (Exception)
        {
            return View("Error");
        }
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }
        catch (Exception)
        {
            return View("Error");
        }
    }

    public IActionResult Edit(int id)
    {
        try
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        catch (Exception)
        {
            return View("Error");
        }
    }

    [HttpPost]
    public IActionResult Edit(int id, Employee employee)
    {
        try
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(employee);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }
        catch (Exception)
        {
            return View("Error");
        }
    }

    public IActionResult Delete(int id)
    {
        try
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        catch (Exception)
        {
            return View("Error");
        }
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            var employee = _context.Employees.Find(id);
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            return View("Error");
        }
    }
}