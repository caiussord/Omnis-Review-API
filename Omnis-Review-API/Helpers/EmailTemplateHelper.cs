namespace OmnisReview.Helpers;

public static class EmailTemplateHelper
{
    public static string GetForgotPasswordEmailBody(string username, string resetLink)
    {
        var html = @"<!DOCTYPE html>
<html lang=""pt-BR"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Redefinição de Senha - Omnis Review</title>
    <link href=""https://fonts.googleapis.com/css2?family=Ubuntu:wght@400;500;700&display=swap"" rel=""stylesheet"">
</head>
<body style=""font-family: 'Ubuntu', sans-serif; margin: 0; padding: 0; background-color: #f5f7fa;"">
    <table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #f5f7fa; padding: 20px 0;"">
        <tr>
            <td align=""center"" style=""padding: 20px;"">
                <!-- Email Container -->
                <table width=""600"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #ffffff; border-radius: 12px; box-shadow: 0 10px 40px rgba(0, 0, 0, 0.1); overflow: hidden;"">

                    <!-- Header com gradiente roxo e verde -->
                    <tr>
                        <td style=""background: linear-gradient(135deg, #3e6b00d6 0%, #2d006bdb 100%); padding: 40px 20px; text-align: center; position: relative; vertical-align: middle;"">
                            <!-- Logo - AUMENTADA -->
                            <div style=""margin-bottom: 30px;"">
                                <img src=""https://raw.githubusercontent.com/caiussord/Omnis-Review-Front/a3d7b1e845866e9fcc9a3d1cdd35c0ce2545e0f9/public/assets/LogoOmnis.png"" alt=""Logo Omnis Review"" style=""width: 120px; height: auto; display: inline-block; margin: 0 auto;"" onerror=""this.style.display='none'"">
                            </div>
                            <!-- Título -->
                            <h1 style=""color: #ffffff; font-size: 28px; font-weight: 700; margin: 0; letter-spacing: -0.5px; font-family: 'Ubuntu', sans-serif;"">Omnis Review</h1>
                        </td>
                    </tr>

                    <!-- Conteúdo -->
                    <tr>
                        <td style=""padding: 40px 30px; font-family: 'Ubuntu', sans-serif;"">
                            <!-- Saudação -->
                            <p style=""color: #2d006bdb; font-size: 16px; margin: 0 0 20px 0; line-height: 1.6; font-family: 'Ubuntu', sans-serif;"">
                                Olá <span style=""color: #3e6b00d6; font-weight: 600;"">{username}</span>,
                            </p>

                            <!-- Mensagem principal -->
                            <p style=""color: #555555; font-size: 14px; line-height: 1.8; margin: 0 0 30px 0; text-align: justify; font-family: 'Ubuntu', sans-serif;"">
                                Recebemos uma solicitação de redefinição de senha para sua conta no Omnis Review. 
                                Para resetar sua senha e recuperar o acesso à sua conta, clique no botão abaixo:
                            </p>

                            <!-- Botão CTA -->
                            <div style=""text-align: center; margin: 35px 0;"">
                                <a href=""{reset_link}"" style=""display: inline-block; background: linear-gradient(135deg, #3e6b00d6 0%, #2d006bdb 100%); color: #ffffff !important; text-decoration: none; font-weight: 700; font-size: 16px; letter-spacing: 0.5px; font-family: 'Ubuntu', sans-serif; line-height: 1.4; padding: 15px 40px; border-radius: 6px; box-shadow: 0 4px 15px rgba(45, 0, 107, 0.3);"">Redefinir Minha Senha</a>
                            </div>

                            <!-- Link alternativo -->
                            <div style=""background-color: #f8f9fa; padding: 20px; border-radius: 8px; margin-top: 20px; text-align: center; border: 1px solid #e9ecef;"">
                                <p style=""color: #888888; font-size: 12px; margin: 0 0 8px 0; font-family: 'Ubuntu', sans-serif;"">Ou copie e cole este link no seu navegador:</p>
                                <a href=""{reset_link}"" style=""color: #3e6b00d6; text-decoration: none; word-break: break-all; font-size: 12px; font-weight: 500; font-family: 'Ubuntu', sans-serif;"">{reset_link}</a>
                            </div>

                            <!-- Aviso de segurança -->
                            <div style=""background-color: #f0f8ff; border-left: 4px solid #3e6b00d6; padding: 15px; margin: 25px 0; border-radius: 4px; font-size: 13px; color: #444444; line-height: 1.6; font-family: 'Ubuntu', sans-serif;"">
                                <strong style=""color: #2d006bdb;"">⚠️ Importante:</strong> Este link é válido por apenas <strong>24 horas</strong>. 
                                Se você não solicitou esta redefinição, por favor ignore este email e sua senha permanecerá inalterada.
                            </div>

                            <!-- Divisor -->
                            <div style=""height: 2px; background: linear-gradient(90deg, transparent, #3e6b00d6, transparent); margin: 20px 0;""></div>

                            <!-- Mensagem de suporte -->
                            <p style=""font-size: 13px; color: #777777; margin-top: 20px; line-height: 1.8; font-family: 'Ubuntu', sans-serif;"">
                                Se você tiver dúvidas ou não solicitou esta redefinição de senha, entre em contato com nossa equipe de suporte 
                                imediatamente. Sua segurança é nossa prioridade.
                            </p>
                        </td>
                    </tr>

                    <!-- Rodapé com fundo cinza -->
                    <tr>
                        <td style=""background-color: #f8f9fa; padding: 30px; text-align: center; border-top: 1px solid #e9ecef; font-family: 'Ubuntu', sans-serif;"">
                            <p style=""color: #3e6b00d6; font-weight: 600; font-size: 14px; margin: 0 0 10px 0; font-family: 'Ubuntu', sans-serif;"">
                                Atenciosamente,<br>
                                <span style=""color: #3e6b00d6; font-weight: 700;"">Equipe Omnis Review</span>
                            </p>

                            <p style=""color: #999999; font-size: 11px; line-height: 1.8; margin: 20px 0 0 0; font-family: 'Ubuntu', sans-serif;"">
                                © 2026 Omnis Review. Todos os direitos reservados.<br>
                                Este é um email automático, por favor não responda este email.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";

        return html.Replace("{username}", username).Replace("{reset_link}", resetLink);
    }
}
