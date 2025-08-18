# KnowledgeTracker

Uma aplicação moderna e multiplataforma para gestão de conhecimento, desenvolvida em .NET MAUI. Organize, acompanhe e consulte soluções técnicas, excertos de código e notas de aprendizagem, com uma interface elegante e adaptável ao tema claro/escuro. Compatível com computadores (Windows, macOS) e dispositivos móveis (Android, iOS).

---

## 🚀 Funcionalidades

- Adicionar, editar, eliminar e pesquisar registos de conhecimento
- Alternância entre tema claro e escuro com ícone dinâmico (sol/lua)
- **Visualização de vídeos do YouTube na aplicação:** insira o URL de um vídeo e assista sem sair da aplicação, ou abra no navegador
- Interface multilingue (Português, English)
- Validação e apresentação de erros
- Layout responsivo para computador e dispositivos móveis
- Estilos personalizáveis através de ficheiros XAML de temas

---

## 🛠️ Primeiros Passos

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) com o workload .NET MAUI

### Instalação

1. **Clone o repositório:**
git clone https://github.com/fauxtix/KnowledgeTracker.git cd KnowledgeTracker


2. **Abra a solução no Visual Studio.**

3. **Restaure os pacotes NuGet.**

4. **Adicione os ícones de tema:**
   - Coloque `moon.png` e `sun.png` em `Resources/Images/`
   - Defina o Build Action como `MauiImage`

5. **Compile e execute o projeto.**

---

## ✨ Utilização

- **Alternar Tema:** Clique no ícone de sol/lua no topo da aplicação.
- **Adicionar Registo:** Preencha o formulário e clique em "Adicionar".
- **Editar Registo:** Selecione um registo, altere os campos e clique em "Atualizar".
- **Eliminar Registo:** Selecione um registo e clique em "Eliminar".
- **Pesquisar:** Utilize a barra de pesquisa para filtrar registos.
- **Visualizar vídeos do YouTube:**
  1. Cole o URL de um vídeo do YouTube no campo "URL do vídeo do YouTube".
  2. Clique em "Exibir Vídeo" para assistir ao vídeo diretamente na aplicação.
  3. Clique em "Ver no YouTube" para abrir o vídeo no navegador.

---

## 🧩 Personalização

- **Temas:** Edite `Resources/Themes/LightTheme.xaml` e `DarkTheme.xaml` para cores e estilos.
- **Ícones:** Substitua `moon.png` e `sun.png` em `Resources/Images/` por ícones personalizados.
- **Idiomas:** Adicione mais idiomas em `IdiomasSuportados` no ViewModel.

---

## 🤝 Contribuição

Contribuições são bem-vindas!  
- Faça fork do repositório
- Crie uma branch de funcionalidade (`git checkout -b feature/NovaFuncionalidade`)
- Faça commit das alterações (`git commit -m 'Adicionar nova funcionalidade'`)
- Faça push para a branch (`git push origin feature/NovaFuncionalidade`)
- Abra um Pull Request

---

## 📄 Licença

Este projeto está licenciado sob a Licença MIT.

---

## 💡 Créditos

Desenvolvido com [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/introduction), .NET MAUI e dedicação à partilha de conhecimento.
