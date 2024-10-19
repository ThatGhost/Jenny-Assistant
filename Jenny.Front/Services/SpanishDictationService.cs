using System.Transactions;

using Jenny.Core;

namespace Jenny.front.Services
{
    public class SpanishDictationService
    {
        private readonly DictationBuilder _builder;
        private readonly SpeechRecognizer _speechRecognizer;
        private readonly Random random;
        private readonly LogService _logService;

        public SpanishDictationService(
            DictationBuilder dictationBuilder,
            SpeechRecognizer speechRecognizer,
            LogService logService
            )
        {
            _builder = dictationBuilder;
            _speechRecognizer = speechRecognizer;
            _logService = logService;
            random = new Random();
        }

        public string[] GetAllDictationsOptions()
        {
            return dictationWords.Select(word => word.Value).ToArray();
        }

        private string correctTranslation = "";
        public void startTranslations()
        {
            _builder.ClearDictations();
            _builder.AddScenentences(GetAllDictationsOptions(), saidWordTranslation);
            _builder.AddScentence("No se", saidWordTranslation);
            _speechRecognizer.UpdateGrammar();

            KeyValuePair<string, string> translation = getRandomWord();
            correctTranslation = translation.Value;
            _logService.LogWithColor(translation.Key, ConsoleColor.White);
        }

        private string correctDictation = "";
        public void startDictation()
        {
            _builder.ClearDictations();
            _builder.AddScenentences(GetAllDictationsOptions(), saidWordDictation);
            _speechRecognizer.UpdateGrammar();

            KeyValuePair<string, string> translation = getRandomWord();
            correctDictation = translation.Value;
            _logService.LogWithColor($"{translation.Value} ({translation.Key})", ConsoleColor.White);
        }

        private void saidWordTranslation(string word)
        {
            if (word == "No se")
            {
                _logService.LogWithColor(correctTranslation, ConsoleColor.Green);
                word = correctTranslation;
            }

            if (word == correctTranslation)
            {
                KeyValuePair<string, string> translation = getRandomWord();
                correctTranslation = translation.Value;
                _logService.LogWithColor(translation.Key, ConsoleColor.White);
            }
            else
            {
                _logService.LogWithColor("No es correcto", ConsoleColor.Red);
            }
        }

        private void saidWordDictation(string word)
        {
            if (word == correctDictation)
            {
                KeyValuePair<string, string> translation = getRandomWord();
                correctDictation = translation.Value;
                _logService.LogWithColor($"{translation.Value} ({translation.Key})", ConsoleColor.White);
            }
            else
            {
                _logService.LogWithColor("No es correcto", ConsoleColor.Red);
            }
        }

        private KeyValuePair<string, string> getRandomWord() => dictationWords[random.Next(dictationWords.Length)];

        private KeyValuePair<string, string>[] dictationWords = new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("one hundred", "cien"),
            new KeyValuePair<string, string>("two hundred", "doscientos"),
            new KeyValuePair<string, string>("three hundred", "trescientos"),
            new KeyValuePair<string, string>("four hundred", "cuatrocientos"),
            new KeyValuePair<string, string>("five hundred", "quinientos"),
            new KeyValuePair<string, string>("six hundred", "seiscientos"),
            new KeyValuePair<string, string>("seven hundred", "setecientos"),
            new KeyValuePair<string, string>("eight hundred", "ochocientos"),
            new KeyValuePair<string, string>("nine hundred", "novecientos"),
            new KeyValuePair<string, string>("Second", "Segundo"),
            new KeyValuePair<string, string>("Minute", "Minuto"),
            new KeyValuePair<string, string>("Hour", "Hora"),
            new KeyValuePair<string, string>("Day", "Día"),
            new KeyValuePair<string, string>("Week", "Semana"),
            new KeyValuePair<string, string>("Month", "Mes"),
            new KeyValuePair<string, string>("Trimester", "Trimestre"),
            new KeyValuePair<string, string>("Semester", "Semestre"),
            new KeyValuePair<string, string>("Year", "Año"),
            new KeyValuePair<string, string>("Century", "Siglo"),
            new KeyValuePair<string, string>("Monday", "Lunes"),
            new KeyValuePair<string, string>("Tuesday", "Martes"),
            new KeyValuePair<string, string>("Wednesday", "Miércoles"),
            new KeyValuePair<string, string>("Thursday", "Jueves"),
            new KeyValuePair<string, string>("Friday", "Viernes"),
            new KeyValuePair<string, string>("Saturday", "Sábado"),
            new KeyValuePair<string, string>("Sunday", "Domingo"),
            new KeyValuePair<string, string>("Morning", "Mañana"),
            new KeyValuePair<string, string>("Midday", "Mediodía"),
            new KeyValuePair<string, string>("Afternoon", "Tarde"),
            new KeyValuePair<string, string>("Night", "Noche"),
            new KeyValuePair<string, string>("Dawn", "Madrugada"),
            new KeyValuePair<string, string>("Hall", "Recibidor"),
            new KeyValuePair<string, string>("Kitchen", "Cocina"),
            new KeyValuePair<string, string>("Dining room", "Comedor"),
            new KeyValuePair<string, string>("Living room", "Salón"),
            new KeyValuePair<string, string>("Bathroom", "Baño"),
            new KeyValuePair<string, string>("Bathroom", "Lavabo"),
            new KeyValuePair<string, string>("Bedroom", "Dormitorio"),
            new KeyValuePair<string, string>("Study", "Estudio"),
            new KeyValuePair<string, string>("Garden", "Jardín"),
            new KeyValuePair<string, string>("Courtyard", "Patio"),
            new KeyValuePair<string, string>("Terrace", "Terraza"),
            new KeyValuePair<string, string>("Balcony", "Balcón"),
            new KeyValuePair<string, string>("Garage", "Garage"),
            new KeyValuePair<string, string>("Room", "Habitación"),
            new KeyValuePair<string, string>("Bed", "Cama"),
            new KeyValuePair<string, string>("Table", "Mesa"),
            new KeyValuePair<string, string>("Desk", "Escritorio"),
            new KeyValuePair<string, string>("Chair", "Silla"),
            new KeyValuePair<string, string>("Toilet", "Váter"),
            new KeyValuePair<string, string>("Couch", "Sofá"),
            new KeyValuePair<string, string>("Curtains", "Cortinas"),
            new KeyValuePair<string, string>("Blind", "Persiana"),
            new KeyValuePair<string, string>("Cutlery", "Cubiertos"),
            new KeyValuePair<string, string>("Fork", "Tenedor"),
            new KeyValuePair<string, string>("Knife", "Cuchillo"),
            new KeyValuePair<string, string>("Spoon", "Cuchara"),
            new KeyValuePair<string, string>("Dish", "Plato"),
            new KeyValuePair<string, string>("Glass", "Vaso"),
            new KeyValuePair<string, string>("Glass", "Copa"),
            new KeyValuePair<string, string>("Pot", "Olla"),
            new KeyValuePair<string, string>("Pan", "Sartén"),
            new KeyValuePair<string, string>("Trip", "Viaje"),
            new KeyValuePair<string, string>("Luggage", "Maleta"),
            new KeyValuePair<string, string>("Holidays", "Vacaciones"),
            new KeyValuePair<string, string>("Movie theater", "Cine"),
            new KeyValuePair<string, string>("Theater", "Teatro"),
            new KeyValuePair<string, string>("Restaurant", "Restaurante"),
            new KeyValuePair<string, string>("Sport", "Deporte"),
            new KeyValuePair<string, string>("Christmas", "Navidad"),
            new KeyValuePair<string, string>("New Year", "Año Nuevo"),
            new KeyValuePair<string, string>("To cook", "Cocinar"),
            new KeyValuePair<string, string>("To clean", "Limpiar"),
            new KeyValuePair<string, string>("To wash", "Lavar"),
            new KeyValuePair<string, string>("To hang (the clothes)", "Tender (la ropa)"),
            new KeyValuePair<string, string>("To fold (the clothes)", "Doblar (la ropa)"),
            new KeyValuePair<string, string>("To iron (the clothes)", "Planchar (la ropa)"),
            new KeyValuePair<string, string>("To make the bed", "Hacer la cama"),
            new KeyValuePair<string, string>("To take out the garbage", "Sacar / tirar la basura"),
            new KeyValuePair<string, string>("To fry", "Freír"),
            new KeyValuePair<string, string>("To heat up", "Calentar"),
            new KeyValuePair<string, string>("To cool down", "Enfriar"),
            new KeyValuePair<string, string>("To boil", "Hervir"),
            new KeyValuePair<string, string>("To roast", "Asar"),
            new KeyValuePair<string, string>("To beat", "Batir"),
            new KeyValuePair<string, string>("To peel", "Pelar"),
            new KeyValuePair<string, string>("To dance", "Bailar"),
            new KeyValuePair<string, string>("To sing", "Cantar"),
            new KeyValuePair<string, string>("To paint", "Pintar"),
            new KeyValuePair<string, string>("To draw", "Dibujar"),
            new KeyValuePair<string, string>("To run", "Correr"),
            new KeyValuePair<string, string>("To swim", "Nadar"),
            new KeyValuePair<string, string>("To go for a walk", "Pasear"),
            new KeyValuePair<string, string>("To travel", "Viajar"),
            new KeyValuePair<string, string>("To meet with", "Quedar"),
            new KeyValuePair<string, string>("To call", "Llamar por teléfono"),
            new KeyValuePair<string, string>("To text", "Mandar mensaje"),
            new KeyValuePair<string, string>("To be late", "Llegar tarde"),
            new KeyValuePair<string, string>("To be busy", "Estar ocupado"),
            new KeyValuePair<string, string>("Good", "Bueno"),
            new KeyValuePair<string, string>("Bad", "Mola"),
            new KeyValuePair<string, string>("Awesome", "Genial"),
            new KeyValuePair<string, string>("Perfect", "Perfecto"),
            new KeyValuePair<string, string>("Horrible", "Horrible"),
            new KeyValuePair<string, string>("Weird", "Raro"),
            new KeyValuePair<string, string>("Boring", "Aburrido"),
            new KeyValuePair<string, string>("Interesting", "Interesante"),
            new KeyValuePair<string, string>("Proud", "Orgulloso"),
            new KeyValuePair<string, string>("Embarrassed", "Avergonzado"),
            new KeyValuePair<string, string>("Full", "Lleno"),
            new KeyValuePair<string, string>("Empty", "Vacío"),
            new KeyValuePair<string, string>("Clean", "Limpio"),
            new KeyValuePair<string, string>("Dirty", "Sucio"),
            new KeyValuePair<string, string>("Broken", "Roto"),
            new KeyValuePair<string, string>("Sick", "Enfermo"),
            new KeyValuePair<string, string>("Healthy", "Sano"),
            new KeyValuePair<string, string>("Married", "Casado"),
            new KeyValuePair<string, string>("Single", "Soltero"),
            new KeyValuePair<string, string>("Alive", "Vivo"),
            new KeyValuePair<string, string>("Dead", "Muerto"),
            new KeyValuePair<string, string>("Cheap", "Barato"),
            new KeyValuePair<string, string>("Expensive", "Caro"),
            new KeyValuePair<string, string>("Easy", "Fácil"),
            new KeyValuePair<string, string>("Difficult", "Difícil"),
            new KeyValuePair<string, string>("Dangerous", "Peligroso"),
            new KeyValuePair<string, string>("Safe", "Seguro"),
            new KeyValuePair<string, string>("Toxic", "Tóxico"),
            new KeyValuePair<string, string>("New", "Nuevo"),
            new KeyValuePair<string, string>("Old", "Viejo"),
            new KeyValuePair<string, string>("Common", "Común"),
            new KeyValuePair<string, string>("Typical", "Típico"),
            new KeyValuePair<string, string>("Useful", "Útil"),
            new KeyValuePair<string, string>("Very", "Muy"),
            new KeyValuePair<string, string>("A lot", "Mucho"),
            new KeyValuePair<string, string>("Less", "Menos"),
            new KeyValuePair<string, string>("Too much", "Demasiado"),
            new KeyValuePair<string, string>("A little", "Poco"),
            new KeyValuePair<string, string>("More", "Más"),
            new KeyValuePair<string, string>("Enough", "Suficiente"),
            new KeyValuePair<string, string>("Near", "Cerca"),
            new KeyValuePair<string, string>("Far", "Lejos"),
            new KeyValuePair<string, string>("Next to", "Al lado"),
            new KeyValuePair<string, string>("Inside", "Dentro de"),
            new KeyValuePair<string, string>("On top of", "Encima de"),
            new KeyValuePair<string, string>("Under", "Debajo de"),
            new KeyValuePair<string, string>("In front of", "Delante"),
            new KeyValuePair<string, string>("Behind", "Detrás"),
            new KeyValuePair<string, string>("Between", "Entre"),
            new KeyValuePair<string, string>("On the right", "A la derecha"),
            new KeyValuePair<string, string>("On the left", "A la izquierda"),
            new KeyValuePair<string, string>("Today", "Hoy"),
            new KeyValuePair<string, string>("Yesterday", "Ayer"),
            new KeyValuePair<string, string>("Tomorrow", "Mañana"),
            new KeyValuePair<string, string>("The day before yesterday", "Anteayer"),
            new KeyValuePair<string, string>("After tomorrow", "Pasado mañana"),
            new KeyValuePair<string, string>("Always", "Siempre"),
            new KeyValuePair<string, string>("Never", "Nunca"),
            new KeyValuePair<string, string>("Now", "Ahora"),
            new KeyValuePair<string, string>("Previously", "Previamente"),
            new KeyValuePair<string, string>("Currently", "Actualmente"),
            new KeyValuePair<string, string>("Already", "Ya"),
            new KeyValuePair<string, string>("In favor of", "Favor de"),
            new KeyValuePair<string, string>("Despite", "A pesar de"),
            new KeyValuePair<string, string>("Except", "Excepto"),
            new KeyValuePair<string, string>("Except", "Salvo"),
            new KeyValuePair<string, string>("Thanks to", "Gracias a"),
            new KeyValuePair<string, string>("Even", "Incluso"),
            new KeyValuePair<string, string>("By", "Por"),
            new KeyValuePair<string, string>("For", "Para"),
            new KeyValuePair<string, string>("According to", "Según"),
            new KeyValuePair<string, string>("Without", "Sin"),
            new KeyValuePair<string, string>("Than", "Que"),
            new KeyValuePair<string, string>("As if", "Como si"),
            new KeyValuePair<string, string>("Without", "Sin que"),
            new KeyValuePair<string, string>("Nevertheless / However", "Sin embargo"),
            new KeyValuePair<string, string>("Indeed", "En efecto"),
            new KeyValuePair<string, string>("Even so", "Con todo"),
            new KeyValuePair<string, string>("That’s why", "Por eso"),
            new KeyValuePair<string, string>("In the first place", "En primer lugar"),
            new KeyValuePair<string, string>("In the second place", "En segundo lugar"),
            new KeyValuePair<string, string>("However / Having said that", "Ahora bien"),
            new KeyValuePair<string, string>("In that case", "En ese caso"),
            new KeyValuePair<string, string>("In spite of that", "A pesar de ello"),
            new KeyValuePair<string, string>("On the contrary", "Por el contrario"),
        };
    }
}
