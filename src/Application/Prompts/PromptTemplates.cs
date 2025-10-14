namespace BotWhatsapp.Application.Prompts;

public static class PromptTemplates
{
    /// <summary>
    /// Prompt para extraer una fecha de un mensaje del usuario.
    /// </summary>
    /// <param name="userMessage">El mensaje original escrito por el usuario.</param>
    public static string DateExtraction(string userMessage) =>
        $"Hoy es {DateTime.Today.ToString()},Extraé una única fecha del siguiente mensaje de usuario: '{userMessage}'. " +
        "La fecha puede estar escrita en lenguaje natural (por ejemplo: 'el viernes que viene', 'el 3 de agosto', 'quiero para hoy', 'quiero para mañana', 'para hoy' etc). " +
        "Recorda que estamos en ARGENTINA" +
        "Si encontrás una fecha válida, respondé solo con la fecha exacta en formato 'dd/MM/yyyy', sin ningún texto adicional. " +
        "Si no hay una fecha clara o no es posible interpretarla, respondé únicamente con 'INVALID'.";


    public static string IsValidRegards(string message) => $"Necesito que por True o False me digas si el usuario esta saludando " +
        $"(puede decirte Hola, buenas, buen dia, buenas tardes, buenas noches o similar, etc (recorda que estamos en argentina)). " +
        $"Mensaje: {message}";


    public static string ClassifyGreeting(string message) =>
    "Analizá el siguiente mensaje del usuario. " +
    "Debés responder en formato JSON con dos campos: " +
    "{ \"classification\": \"InitialGreeting|CourtesyGreeting|Other\", \"name\": \"<nombre extraído o vacío>\" }. " +
    "\n\nReglas:" +
    "\n- Si el mensaje es exactamente 'volver' (ignorando mayúsculas/minúsculas), classification = \"InitialGreeting\" y name = \"\"." +
    "\n- Si el mensaje es un saludo inicial (\"hola\", \"buenas\", \"buen día\", \"buenas tardes\", \"buenas noches\"), classification = \"InitialGreeting\" y name = \"\"." +
    "\n- Si el mensaje es un saludo de cortesía (\"cómo estás\", \"todo bien\", \"qué tal\", \"cómo va\"), classification = \"CourtesyGreeting\" y name = \"\"." +
    "\n- Si el mensaje no es saludo (Other) pero parece contener un nombre (ej: \"me llamo Juan Pérez\", \"soy Carla\", \"mi nombre es Martín\"), classification = \"Other\" y name debe contener el nombre detectado." +
    "\n- Si es Other y no hay nombre claro, name = \"\"." +
    $"\n\nMensaje: {message}";

    public static string ConversationalPrompt(string message) => $@"
    El contexto es el siguiente, estamos en un chat de WhatsApp entre un asistente virtual y un usuario, es una conversación informal, 
    todavia no tenemos el nombre del usuario, asiq necesitamos obtenerlo, la idea es ayudar al usuario a poder llevar a cabo una reserva de turnos, 
    poder consultar sus turnos, cancelarlos y reprogramarlos. Sos un asistente virtual simpático y útil. Respondé de manera personalizada según el contexto. 
    Si el usuario pide información general, sé breve y claro. Si pide ayuda o consejo, explicá más en detalle. Si menciona su nombre, recordalo en la conversación. 
    Este es el mensaje del usuario: {message}";

    public static string ConfirmBooking(string message) => $"Necesito que por True o False me digas si el usuario confirma el turno " +
                    $"(puede decirte Si, sii, si de una, dale! o no, no puedo etc (recorda que estamos en argentina)). Mensaje: {message}";
    /// <summary>
    /// Prompt del menú minimalista con las opciones del bot de turnos.
    /// </summary>
    /// <param name="name">Nombre del usuario que chatea con el bot.</param>
    public static string OptionsMenu(string name) =>
        $"👋 Hola *{name}*, ¿qué querés hacer?\n" +
        "1️⃣ *Crear turno*\n" +
        "2️⃣ *Reprogramar turno*\n" +
        "3️⃣ *Cancelar turno*\n" +
        "4️⃣ *Ver turnos*\n\n" +
        "✏️ Escribí el número de la opción.\n" +
        "🧭 Escribí *volver* en cualquier momento para cancelar.";

    public static string ShowMainMenu()
    {
        return $"👋 ¿Qué querés hacer?\n" +
        "1️⃣ *Crear turno*\n" +
        "2️⃣ *Reprogramar turno*\n" +
        "3️⃣ *Cancelar turno*\n" +
        "4️⃣ *Ver turnos*\n" +
        "✏️ Escribí el número de la opción.\n" +
        "🧭 Escribí *volver* en cualquier momento para cancelar.";
    }

    /// <summary>
    /// Prompt para preguntar al usuario que dia quiere reservar el turno.
    /// </summary>
    public static string AnswerDay() 
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var dayAfterTomorrow = today.AddDays(2);
        var threeDaysLater = today.AddDays(3);
        var fourDaysLater = today.AddDays(4);
        var fiveDaysLater = today.AddDays(5);
        var sixDaysLater = today.AddDays(6);
        var sevenDaysLater = today.AddDays(7);

        return
            $" ¿Para qué día querés reservar?{Environment.NewLine}" +
            $" Recorda que los turnos son de 1h 30min {Environment.NewLine}" +
            $"1️⃣ {today:dddd, dd/MM} (hoy){Environment.NewLine}" +
            $"2️⃣ {tomorrow:dddd, dd/MM} (mañana){Environment.NewLine}" +
            $"3️⃣ {dayAfterTomorrow:dddd, dd/MM}{Environment.NewLine}" +
            $"4️⃣ {threeDaysLater:dddd, dd/MM}{Environment.NewLine}" +
            $"5️⃣ {fourDaysLater:dddd, dd/MM}{Environment.NewLine}" +
            $"6️⃣ {fiveDaysLater:dddd, dd/MM}{Environment.NewLine}" +
            $"7️⃣ {sixDaysLater:dddd, dd/MM}{Environment.NewLine}" +
            $"O escribe 'otros' para ingresar otra fecha.";
    }
}
