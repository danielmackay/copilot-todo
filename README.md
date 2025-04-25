# Copilot Todo Application

A full-stack Todo application built with React (TypeScript) frontend and .NET 9 API backend.

## Project Structure

- `/frontend` - React TypeScript application with TailwindCSS
- `/api` - .NET 9 API with ASP.NET Core Minimal APIs

## Setup Instructions

### Prerequisites

- Node.js (v18+)
- npm or yarn
- .NET 9 SDK

### Frontend Setup

1. Navigate to the frontend directory:
   ```
   cd frontend
   ```

2. Install dependencies:
   ```
   npm install
   ```

3. Start the development server:
   ```
   npm run dev
   ```

4. The application will be available at http://localhost:5173

### Backend Setup

1. Navigate to the API directory:
   ```
   cd api
   ```

2. Restore .NET packages:
   ```
   dotnet restore
   ```

3. Run the API:
   ```
   dotnet run
   ```

4. The API will be available at http://localhost:5000

## Features

- Create, read, update, and delete Todo items
- Mark Todo items as complete
- Clean, modern UI built with React and TailwindCSS
- RESTful API with ASP.NET Core
- In-memory database with Entity Framework Core