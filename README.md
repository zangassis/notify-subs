# 📧 NotifySubs: Subscription Notification System

**This project contains a sample ASP.NET Core app. This app is an example of the article I produced for the Telerik Blog (telerik.com/blogs)**

NotifySubs is an ASP.NET Core application designed to notify users about their subscription and contract expirations. It leverages **Hangfire** to schedule and manage jobs efficiently, and uses **SQL Server** with **Entity Framework Core** to store and manage data. 🚀

## 🌟 Features
- **🔁 Recurring Notifications**: Sends regular notifications for upcoming expirations.
- **⏳ Delayed Notifications**: Schedules notifications to be sent at a future date.
- **🔥 Fire-and-Forget Notifications**: Instantly sends notifications without waiting for the response.

## 🛠️ Technology Stack
- **ASP.NET Core**: Web API framework used to build the application.
- **Hangfire**: Manages background jobs for recurring, delayed, and fire-and-forget tasks.
- **SQL Server**: Database to store subscription and notification data.
- **Entity Framework Core**: ORM for database interactions with SQL Server.

## 🚀 Getting Started

### Prerequisites
- .NET SDK (version 8.0 or later)
- SQL Server
- Hangfire (set up in the app)

### Installation

1. **Clone the repository**:
    ```bash
    git clone https://github.com/zangassis/notify-subs.git
    cd NotifySubs
    ```

2. **Install dependencies**:
    ```bash
    dotnet restore
    ```

3. **Set up the database**:
    Update your connection string in `appsettings.json`:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=your_sql_server;Database=NotifySubsDb;Trusted_Connection=True;"
    }
    ```

4. **Run EF Core migrations**:
    ```bash
    dotnet ef database update
    ```

5. **Run the application**:
    ```bash
    dotnet run
    ```

## 🏗️ Architecture Overview
The NotifySubs application is organized into three main job types, each responsible for sending notifications using **Hangfire**:

- **Recurring Notifications**: Jobs that send periodic notifications to users before the expiration date. Ideal for weekly or monthly reminders.
  
- **Delayed Notifications**: Jobs that send notifications at a specified future time, perfect for sending a warning just before expiration.

- **Fire-and-Forget Notifications**: Jobs that trigger immediately when a subscription or contract was renewed.

## 📚 Usage

After running the application, you can monitor the jobs using Hangfire Dashboard. To access it, navigate to:

```
http://localhost:PORT/hangfire
```

Here you can see job statistics, retries, failures, and the current job queue.

## 🔧 Configuration

The following configuration options are available in `appsettings.json`:

- **SQL Server Connection String**: Configure your database connection.
- **Hangfire Settings**: Adjust Hangfire options such as the polling interval, job retries, and more.

## 📝 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
