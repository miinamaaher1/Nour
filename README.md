# Nour

![Logo](XCourse.Web/wwwroot/docs/designs/NOUR.png)

**Nour** is an educational platform designed to connect **teachers**, **students**, **assistants**, and **center admins**, streamlining and supporting the private tutoring business.

This project was developed by our team as part of the [ITI](https://iti.gov.eg/home) ASP.NET MVC course under the Integrated Software Development & Architecture track.

## Team

- [@miinamaaher1](https://github.com/miinamaaher1)
- [@abdelrahmanramadan12](https://github.com/abdelrahmanramadan12)
- [@AhmedYasserMohammed](https://github.com/AhmedYasserMohammed)
- [@shroukzaghloul2024](https://github.com/shroukzaghloul2024)
- [@MahmoudRKeshk](https://github.com/MahmoudRKeshk)

## Technology

### ASP.NET Core MVC

![Landing Page](XCourse.Web/wwwroot/docs/screenshots/00%20landing.png)
Built using **ASP MVC**, providing a robust framework for web app development with server-side rendering, routing, and controller-based logic.

### Entity Framework Core & Microsoft SQL Server

We use **Entity Framework** as an ORM with **SQL Server** for seamless data access and management.

### ASP.NET Core Identity

![Signin Page](XCourse.Web/wwwroot/docs/screenshots/01%20signin.png)
Handles user authentication and authorization securely with **ASP Identity**.

### Clean Architcture

![Architecture](XCourse.Web/wwwroot/docs/screenshots/10%20archticture.png)
Follows a layered, clean architecture approach for better scalability and maintainability.

### Google Authintication

Supports registration and login using **Google accounts** via OAuth.

### YouTube API

![YuoTube Video](XCourse.Web/wwwroot/docs/screenshots/13%20youtube.png)
Teachers can upload session videos using the **YouTube API** for seamless content delivery.

### Maps API

![Instructor Home](XCourse.Web/wwwroot/docs/screenshots/03%20maps.png)
Integrates **Google Maps API** for location-based services and navigation.

### Gmail API

Used to send **email confirmations** and **password reset codes** directly to users.

### Stripe API

Enables **secure online payments** and wallet top-ups through **Stripe**.

## Platform Overview

The platform is divided into four role-specific user areas, each with distinct features but connected through a shared system with role-based authorization, effectively functioning as four integrated applications.

## Common Features

### Accounts

![Account Types](XCourse.Web/wwwroot/docs/screenshots/11%20user-types.png)
Users log in securely and are redirected to their role-specific homepages.

### Theme

![Theme](XCourse.Web/wwwroot/docs/screenshots/09%20dark-mode.png)
Users can choose between light, dark, or system-based themes.

### Wallet

![Instructor Home](XCourse.Web/wwwroot/docs/screenshots/04%20wallet.png)
Each user has a personal wallet that can be topped up via Stripe and used for in-platform payments.

## Teacher Features

![Teacher Home](XCourse.Web/wwwroot/docs/screenshots/12%20teacher-home.png)

### Groups Management

![Groups](XCourse.Web/wwwroot/docs/screenshots/05%20groups.png)

Teachers can:

- Create Online, Offline(center), or Offline(local) groups.
- Book rooms for center-based groups.
- Edit group information.
- Invite assistants.

### Sessions Management

Teachers can:

- Add and edit sessions.
- Book rooms for center-based sessions.
- Upload videos via YouTube API for online sessions.
- Manage student attendance and marks.

### Announcements

![Announcements](XCourse.Web/wwwroot/docs/screenshots/14%20announcements.png)

Teachers can:

- Create and post announcements to selected groups.
- Edit or delete announcements.

## Student Features

![Student Home](XCourse.Web/wwwroot/docs/screenshots/02%20student-home.png)

### Join Groups

Students can:

- Join Online, Offline(center), or Offline(local) groups.
- Receive group announcements and updates.

### Attend Sessions

![Calender](XCourse.Web/wwwroot/docs/screenshots/06%20callender.png)

Students can:

- View sessions in a calendar view.
- Access session details, including location.
- Access video playback for online sessions.
- Pay online for sessions.

### Book Study Rooms

![Instructor Home](XCourse.Web/wwwroot/docs/screenshots/15%20watch.png)

Students can:

- Browse available centers and rooms.
- Book study rooms as needed.

## Assistant Features

![Instructor Home](XCourse.Web/wwwroot/docs/screenshots/07%20assistant-home.png)

### Group Management

Assistants can:

- Accept or decline teacher invitations.
- Manage student attendance and marks in assigned groups.

## Center Admin Features

![Instructor Home](XCourse.Web/wwwroot/docs/screenshots/08%20center-home.png)

### Center Management

Admin can:

- Create centers.
- Edit center's info.
- Delete center if it has no rooms.

### Room Management

Admin can:

- Create rooms.
- Edit room's info.
- Delete room if it has no reservations.

### Reservation Management

Admins can:

- Accept or decline room reservations.
- View reservation statistics.
- Modify reservations to suitable rooms.
- Delete and refund reservations when necessary.

## Setup Steps

1. **Clone the Repository**

   ```sh
   git clone <https://github.com/miinamaaher1/Nour>
   cd Nour
   ```

2. **Configure the Database**

- Update the `appsettings.json` file with your database **connection string**.

3. **Configure apis**
   - Add your **API keys** and **refresh tokens** in the `appsettings.json` file.

4. **Build & Run the Application**
   - Open the solution in Visual Studio.
   - Restore dependencies using:

     ```sh
     dotnet restore
     ```

   - Build and run the project:

     ```sh
     dotnet run
     ```
