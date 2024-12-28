import kotlinx.serialization.Serializable
import kotlinx.serialization.encodeToString
import kotlinx.serialization.json.Json

@Serializable
data class UserBasic(
    val name: String,
    val age: Int
)

fun main() {
    val jsonString = """{"name":"Alice","age":25}"""
    val user = Json.decodeFromString<UserBasic>(jsonString)
    println("Имя: ${user.name}, возраст: ${user.age}") // Имя: Alice, возраст: 25

    val user2 = UserBasic("Bob", 30)
    val resultJson = Json.encodeToString(user2)
    println(resultJson) // {"name":"Bob","age":30}
}