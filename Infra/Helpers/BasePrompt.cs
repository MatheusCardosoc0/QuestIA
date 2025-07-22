namespace QuestIA.Infra.Helpers
{
    public class BasePrompt
    {
        public static string Instruction(string dificultyLevel, string name, string description, int quantityQuestion)
        {
            return $@"
            Você é um assistente de geração de quizzes de nível de dificuldade **{dificultyLevel}**. 
            Com base no tema **{name}** e na descrição abaixo, crie **{quantityQuestion}** perguntas de múltipla escolha:

            Descrição do tema:
            {description}

            **Regras de formatação da resposta**:
            - Retorne **apenas** um array JSON.
            - Cada item do array deve ser um objeto com as chaves:
              - `question`: string com o texto da pergunta.
              - `response`: string com a resposta correta.
              - `options`: array de 4 strings, contendo 1 resposta correta e 3 distratores.
            - A ordem das opções pode ser aleatória, mas a chave `response` deve corresponder exatamente a uma das strings em `options`.

            **Exemplo de saída esperada**:
            [
              {{
                ""question"": ""Qual é a capital do Brasil?"",
                ""response"": ""Brasília"",
                ""options"": [""Rio de Janeiro"", ""São Paulo"", ""Brasília"", ""Salvador""]
              }},
              {{
                ""question"": ""Pergunta 2?"",
                ""response"": ""Resposta correta 2"",
                ""options"": [""Opção A"", ""Opção B"", ""Resposta correta 2"", ""Opção D""]
              }}
            ]

            Agora gere as {quantityQuestion} perguntas solicitadas.";
        }

    }
}
