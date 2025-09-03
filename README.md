# KnowledgeTracker

Um pequeno programa, que tem como principal objetivo, a 'gestão de conhecimento'; desenvolvido em .NET MAUI/Desktop (Windows e macOS), usando a linguagem C#. 

Permite a orgnização, acompanhamento e consulta de soluções técnicas, excertos de código e notas de aprendizagem, para futura referenciação.

Interface adaptável ao tema claro/escuro.


---

## 🚀 Funcionalidades

- Adicionar, editar, eliminar e pesquisar
- Alternância entre tema claro e escuro com ícone dinâmico (sol/lua)
- ** Visualização de vídeos do YouTube na aplicação:
 -- insira o URL de um vídeo e assista sem sair da aplicação, ou abra no navegador
- Validação e apresentação de erros
- Estilos personalizáveis através de ficheiros XAML de temas

---

## 🛠️ Primeiros Passos

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) com o workload .NET MAUI

### Instalação

1. **Clone o repositório:**
git clone https://github.com/fauxtix/KnowledgeTracker.git cd KnowledgeTracker

2. **Compile e execute o projeto.**

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

## 📷 Screenshots

<img width="1426" height="752" alt="MainPage_1" src="https://github.com/user-attachments/assets/01dbe263-4d01-4d91-9973-3c6468958d23" />
<img width="1426" height="752" alt="MainPage_2" src="https://github.com/user-attachments/assets/cb7f5876-3802-4bd7-9a8f-3bc5ed779712" />
<img width="1426" height="752" alt="MainPage_1_Ligth" src="https://github.com/user-attachments/assets/33f99a7d-feaf-4ade-a345-880305cf3a27" />
<img width="1426" height="752" alt="MainPage_2_Ligth" src="https://github.com/user-attachments/assets/de66ee63-4376-45ed-856a-c78448d7afc6" />


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
