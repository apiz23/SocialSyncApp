# SocialSync Mobile App

![.NET MAUI](https://img.shields.io/badge/.NET_MAUI-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Supabase](https://img.shields.io/badge/Supabase-3ECF8E?style=for-the-badge&logo=supabase&logoColor=white)

**SocialSyncApp** is a cross-platform mobile social networking application built with **.NET MAUI**. It allows users to connect, share updates, manage events, and maintain a friends list in real-time using a Supabase backend.

🔗 **Repository:** [https://github.com/apiz23/SocialSyncApp](https://github.com/apiz23/SocialSyncApp)

## ✨ Features

* **🔐 Authentication:** Secure Login and Registration system.
* **📝 Social Feed:** View, Create, Edit, and Delete posts with rich text and image support.
* **📅 Event Management:**
    * Create and host events.
    * View upcoming events with date, time, and location details.
    * Creator-only controls (Edit/Delete).
* **👥 Friends System:**
    * View friend list.
    * Search for new users.
    * Add or Unfriend users.
    * View user profiles.
* **👤 Profile Management:** View personal stats (Friends count, Posts count) and join date.

## 🛠️ Tech Stack

* **Framework:** .NET MAUI (Multi-platform App UI)
* **Language:** C#
* **Architecture:** MVVM (Model-View-ViewModel)
* **Backend/Database:** Supabase (PostgreSQL)
* **Authentication:** BCrypt (Password Hashing) & Supabase Auth
* **UI Components:** XAML, CollectionView, Custom Controls

## 🚀 Getting Started

### Prerequisites
* [Visual Studio 2022](https://visualstudio.microsoft.com/) (with .NET Multi-platform App UI development workload).
* .NET 7.0 or 8.0 SDK.

### Installation

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/apiz23/SocialSyncApp.git](https://github.com/apiz23/SocialSyncApp.git)
    ```
2.  **Open the solution:**
    Open `SocialSyncApp.sln` in Visual Studio.

3.  **Configure Backend:**
    * Locate `SupabaseService.cs` (or your constants file).
    * Update the `_supabaseUrl` and `_supabaseKey` with your project credentials.

4.  **Restore Nuget Packages:**
    Right-click the solution -> **Restore NuGet Packages**.

5.  **Run the App:**
    Select your target emulator (Android/iOS) or Windows Machine and press **F5**.

## 🗄️ Database Schema (Supabase)

Ensure your Supabase project has the following tables:
* `social_sync_users` (id, fullname, email, password, etc.)
* `social_sync_posts` (id, title, content, author_id, etc.)
* `social_sync_events` (id, title, event_date, creator_id, etc.)
* `social_sync_friends` (user_id, friend_id)

## 📸 Screenshots

| Login | Feed | Events | Friends |
|:---:|:---:|:---:|:---:|
| *(Add Img)* | *(Add Img)* | *(Add Img)* | *(Add Img)* |

## 🤝 Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## 📄 License

[MIT](https://choosealicense.com/licenses/mit/)
