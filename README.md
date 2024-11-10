# Training Record System
This is a .NET 6 and Angular 16 application designed to manage training nominations within an organization. The system enables Admins, Trainers, and Managers to coordinate training sessions by assigning trainers, managing participants, and generating reports.

## Overview
The Training Record System allows managers to nominate team members for organizational training sessions. This system supports various roles with distinct permissions, including Admins, Trainers, and Managers. Admins can assign training topics to Trainers, Trainers can manage training program details, and Managers can nominate their team members for upcoming trainings.

## Tech Stack
- Backend: .NET 6 (ASP.NET Web API)
- Frontend: Angular 16
- Database: SQL Server
- Additional Libraries: Entity Framework Core, Bootstrap

## Features
- Admin Functionalities
1. Add Trainer: Admins can add trainers and assign training topics along with the target job role. Trainers may handle multiple topics but are restricted to their assigned job roles.
2. Monthly Report: Based on a selected trainer, displays all training topics with participant counts, filtered by the start date of each training.
3. Date Range Report: Based on a selected job role, displays training topics, trainers, and participant counts within a specific date range.
- Trainer Functionalities
1. View Assigned Topics: Trainers can view their assigned training topics but cannot modify the topic names.
2. Add Training Program Details: Trainers can define training details such as the target audience, duration, date/time, and mode (Online, Offline, Hybrid).
3. View Participants: Trainers can view participants' information along with their manager’s details, displayed in a paginated and sortable tabular grid.
- Manager Functionalities
1. View Upcoming Trainings: Managers can view all upcoming trainings by job role.
2. Nominate Participants: Managers can nominate up to 5 team members per training session and must provide participant details such as name, email, job role, and training mode. For Hybrid mode, Managers specify if each participant prefers Offline or Online attendance.
