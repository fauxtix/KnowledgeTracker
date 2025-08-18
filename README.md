# KnowledgeTracker

A modern, cross-platform knowledge management app built with .NET MAUI. Track, organize, and revisit your technical solutions, code snippets, and learning notes with a beautiful, theme-aware interface.

---

## 🚀 Features

- Add, edit, delete, and search knowledge entries
- Light & dark theme switching with dynamic sun/moon icon
- YouTube video embedding and browser launching
- Multi-language UI (Português, English)
- Validation & error feedback
- Responsive layout for desktop and mobile
- Customizable styles via XAML theme files

---

## 🛠️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) with .NET MAUI workload

### Setup

1. **Clone the repository:**
git clone https://github.com/yourusername/KnowledgeTracker.git cd KnowledgeTracker


2. **Open the solution in Visual Studio.**

3. **Restore NuGet packages.**

4. **Add theme icons:**
   - Place `moon.png` and `sun.png` in `Resources/Images/`
   - Set their Build Action to `MauiImage`

5. **Build and run the project.**

---

## ✨ Usage

- **Switch Theme:** Tap the sun/moon icon in the header.
- **Add Entry:** Fill out the form and click "Adicionar".
- **Edit Entry:** Select an entry, update fields, and click "Atualizar".
- **Delete Entry:** Select an entry and click "Excluir".
- **Search:** Use the search bar to filter entries.
- **YouTube Integration:** Paste a YouTube URL and use the buttons to embed or open the video.

---

## 📦 Project Structure
KnowledgeTracker/ ├── Resources/ │   ├── Images/                # App icons (sun.png, moon.png, etc.) │   └── Themes/                # LightTheme.xaml, DarkTheme.xaml ├── Models/                    # Data models ├── ViewModels/                # MVVM logic ├── Data/                      # Data access and interfaces ├── MainPage.xaml              # Main UI ├── App.xaml                   # Application resources and startup ├── Program.cs                 # MAUI app entry point └── ...

---

## 🧩 Customization

- **Themes:** Edit `Resources/Themes/LightTheme.xaml` and `DarkTheme.xaml` for colors and styles.
- **Icons:** Replace `moon.png` and `sun.png` in `Resources/Images/` for custom theme icons.
- **Languages:** Add more languages to `IdiomasSuportados` in the ViewModel.

---

## 🤝 Contributing

Contributions are welcome!  
- Fork the repo
- Create your feature branch (`git checkout -b feature/AmazingFeature`)
- Commit your changes (`git commit -m 'Add some AmazingFeature'`)
- Push to the branch (`git push origin feature/AmazingFeature`)
- Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License.

---

## 💡 Credits

Built with [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/introduction), .NET MAUI, and a passion for knowledge sharing.
