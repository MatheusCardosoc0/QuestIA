using QuestIA.Core.Models;

namespace QuestIA.Infra.Helpers
{
    public class BasePrompt
    {
        public static string Instruction(DifficultyLevel difficultyLevel, string name, string description, int quantityQuestions)
        {
            var difficulty = difficultyLevel switch
            {
                DifficultyLevel.Easy => "Easy",
                DifficultyLevel.Medium => "Medium",
                DifficultyLevel.Hard => "Hard",
                _ => throw new ArgumentOutOfRangeException(nameof(difficultyLevel), difficultyLevel, null)
            };

            return $@"
            Você é um assistente de geração de quizzes de nível de dificuldade **{difficulty}**. 
            Com base no tema **{name}** e na descrição abaixo, crie **{quantityQuestions}** perguntas de múltipla escolha:

            Descrição do tema:
            {description}

            **Regras de formatação da resposta**:
            - Retorne **apenas** um array JSON.
            - Cada item do array deve ser um objeto com as chaves:
              - `QuestionText`: string com o texto da pergunta.
              - `Response`: string com a resposta correta.
              - `Options`: array de 4 strings, contendo 1 resposta correta e 3 distratores.
            - A ordem das opções pode ser aleatória, mas a chave `Response` deve corresponder exatamente a uma das strings em `Options`.

            **Exemplo de saída esperada**:
            [
              {{
                ""QuestionText"": ""Qual é a capital do Brasil?"",
                ""Response"": ""Brasília"",
                ""Options"": [""Rio de Janeiro"", ""São Paulo"", ""Brasília"", ""Salvador""]
              }},
              {{
                ""QuestionText"": ""Pergunta 2?"",
                ""Response"": ""Resposta correta 2"",
                ""Options"": [""Opção A"", ""Opção B"", ""Resposta correta 2"", ""Opção D""]
              }}
            ]

            Agora gere as {quantityQuestions} perguntas solicitadas.";
        }

    }
}
