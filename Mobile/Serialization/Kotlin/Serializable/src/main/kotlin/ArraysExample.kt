import kotlinx.serialization.Serializable
import kotlinx.serialization.json.Json

@Serializable
data class UserForArray(
    val id: Int,
    val name: String
)

@Serializable
data class UsersResponse(
    val users: List<UserForArray>
)

fun main() {
    val jsonString = """
        {
          "users": [
            {"id": 1, "name": "Alice"},
            {"id": 2, "name": "Bob"}
          ]
        }
    """.trimIndent()

    // Декодируем список
    val response = Json.decodeFromString<UsersResponse>(jsonString)
    println("Получено пользователей: ${response.users.size}")
    response.users.forEach {
        println("ID: ${it.id}, Name: ${it.name}")
    }

    // Пример, когда просто массив без объекта-обёртки:
    val plainArrayJson = """
        [
          {"id": 10, "name": "Charlie"},
          {"id": 20, "name": "Dana"}
        ]
    """.trimIndent()

    // Декодируем сразу List<UserForArray>, без обёртки
    val plainList = Json.decodeFromString<List<UserForArray>>(plainArrayJson)
    plainList.forEach {
        println("Plain array -> ID=${it.id}, Name=${it.name}")
    }
}