import kotlinx.serialization.Serializable
import kotlinx.serialization.SerialName
import kotlinx.serialization.json.Json

@Serializable
data class StrangeUser(
    @SerialName("user_name")
    val name: String,

    // Приходит как строка, но хотим хранить как Int
    @SerialName("user_age")
    val rawAge: String
) {
    // Кастомное свойство, чтобы преобразовать строку в число
    val age: Int
        get() = rawAge.toIntOrNull() ?: -1
}

fun main() {
    val jsonString = """
        {
          "user_name": "Alice",
          "user_age": "25"
        }
    """.trimIndent()

    val user = Json.decodeFromString<StrangeUser>(jsonString)
    println("Имя: ${user.name}, Возраст: ${user.age}")
}