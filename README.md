# VC Workout

A personal gym workout tracking application to register exercises, workouts, and sets in real-time.

## Overview

This is a personal workout tracking app designed for live gym sessions. You can start a workout, select exercises, add sets with weight/reps/RPE, and view your progress from previous sessions to decide whether to increase weight or reps.

## Tech Stack

- **Backend:** ASP.NET Core Web API
- **Frontend:** Blazor WebAssembly
- **Styling:** Tailwind CSS
- **Data Persistence:** 
  - Backend database (SQL Server/PostgreSQL)
  - LocalStorage (only for active workout ID)

## Core Concepts

### Workout Flow
1. Start a new workout (creates record in database with today's date)
2. The workout becomes "active" (stored in global state)
3. Navigate to exercises and add sets - they automatically link to the active workout
4. Finish the workout when done (marks EndDate)

### Data Model

**Exercise**
- Id
- Name
- CreatedAt

**Workout**
- Id
- Date
- EndDate (null if active)

**WorkoutSet**
- Id
- WorkoutId
- ExerciseId
- SetNumber (1, 2, 3...)
- Weight (in KG)
- Reps
- Rpe (1-10, Rating of Perceived Exertion)
- CreatedAt

## Features

### Dashboard
- Statistics: total workouts, last workout date, sets this month, current streak
- "Start Workout" button (creates new workout and marks as active)
- "Finish Workout" button (when workout is active)

### Exercises Page
- List all exercises
- Create/Edit/Delete exercises
- Click exercise to add a set (shows modal)
- Modal displays last 3 times this exercise was performed

### Workouts Page
- Paginated list of all workouts
- View workout details (all exercises and sets)
- Edit/Delete individual sets
- Delete entire workouts

## API Endpoints

### Exercises
- `GET /api/exercises` - List all exercises
- `POST /api/exercises` - Create exercise `{ name }`
- `PUT /api/exercises/{id}` - Update exercise
- `DELETE /api/exercises/{id}` - Delete exercise

### Workouts
- `GET /api/workouts?page=1&pageSize=10` - List workouts (paginated)
- `GET /api/workouts/{id}` - Get workout with all sets
- `POST /api/workouts` - Create workout `{ date }`
- `PUT /api/workouts/{id}/finish` - Mark as finished
- `DELETE /api/workouts/{id}` - Delete workout

### Sets
- `GET /api/exercises/{exerciseId}/history?limit=3` - Last 3 workouts with this exercise
- `POST /api/sets` - Add set `{ workoutId, exerciseId, setNumber, weight, reps, rpe }`
- `PUT /api/sets/{id}` - Update set
- `DELETE /api/sets/{id}` - Delete set

### Stats
- `GET /api/stats` - Dashboard statistics

## Frontend Structure

```
Components/
├── Layout/
│   ├── MainLayout.razor          # Navigation bar
│   └── NavMenu.razor             # Dashboard | Exercises | Workouts
├── Pages/
│   ├── Dashboard.razor           # Stats + Start/Stop workout
│   ├── Exercises.razor           # CRUD exercises
│   └── Workouts.razor            # View/edit workouts
├── Shared/
│   ├── AddSetModal.razor         # Add set with history
│   └── ExerciseForm.razor        # Exercise name form
└── Services/
    ├── WorkoutStateService.cs    # Global state (active workout)
    ├── ExerciseService.cs        # Exercise API calls
    ├── WorkoutService.cs         # Workout API calls
    └── SetService.cs             # Set API calls
```

## State Management

- **WorkoutStateService:** Singleton service managing active workout ID
- Persists active workout ID to LocalStorage
- On app init, checks LocalStorage and verifies workout is still active via API
- All components inject this service to check workout state

## Development Roadmap

### Phase 1: Backend API
- [ ] Implement Exercise endpoints
- [ ] Implement Workout endpoints
- [ ] Implement Set endpoints
- [ ] Implement Stats endpoint
- [ ] Add validation (RPE 1-10, required fields)

### Phase 2: Frontend Structure
- [ ] Create Blazor WASM project
- [ ] Configure Tailwind CSS
- [ ] Setup HttpClient with API base URL
- [ ] Create service classes
- [ ] Create WorkoutStateService with LocalStorage
- [ ] Build layout and navigation

### Phase 3: Features
- [ ] Exercises page (CRUD)
- [ ] Dashboard with start/finish workout
- [ ] Add set modal with exercise history
- [ ] Workouts page with full CRUD
- [ ] Error handling and validation

## Edge Cases & Error Handling

- Network failures show toast notifications with retry option
- Active workout verified on page reload (checks if still valid)
- Cannot delete exercises with existing sets
- RPE validated on frontend (1-10) and backend (400 error)
- Multiple tabs may have stale state until reload (acceptable for now)

## Future Iterations

- Exercise categories/tags
- Exercise descriptions/notes
- Workout templates
- More advanced statistics and charts
- Exercise history charts
- Rest timer between sets
- PWA installation support
