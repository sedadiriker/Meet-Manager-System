# Meet-Manager-System

ProjectRoot/
│
├── ApiLayer/                    # API controllers and services
│   ├── Controllers/
│   ├── Program.cs
│   ├── Startup.cs
│   └── appsettings.json
│
├── BusinessLayer/               # Business logic services
│   └── Services/
│       └── UserService.cs
│
├── DataAccessLayer/             # Entity Framework DbContext and migrations
│   ├── Migrations/
│   ├── ApplicationDbContext.cs
│   └── IRepository.cs
│
├── EntitiesLayer/               # Database and business model classes
│   ├── User.cs
│   └── Meeting.cs
│
├── PresentationLayer/           # UI components (Angular or MVC)
│   ├── Angular/                 # Angular project files
│   └── MVC/                     # MVC project files
│       ├── Controllers/
│       ├── Views/
│       └── wwwroot/
│
├── docker/                      # Docker-related files
│   ├── Dockerfile
│   └── docker-compose.yml
│
├── .gitignore                   # Git ignore settings
└── README.md                    # Project documentation

