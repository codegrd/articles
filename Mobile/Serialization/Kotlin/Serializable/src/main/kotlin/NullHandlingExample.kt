import kotlinx.serialization.Serializable
import kotlinx.serialization.json.Json
import kotlinx.serialization.json.buildJsonObject
import kotlinx.serialization.json.put

@Serializable
data class UserNullable(
    val name: String,       // Должен прийти
    val age: Int? = null    // Может отсутствовать или быть null
)

fun main() {
    // 1) age = null
    val jsonString1 = """{"name": "Bob", "age": null}"""
    val user1 = Json.decodeFromString<UserNullable>(jsonString1)
    println("User1: name=${user1.name}, age=${user1.age}")
    // name=Bob, age=null

    // 2) поле age отсутствует
    val jsonString2 = """{"name": "Charlie"}"""
    val user2 = Json.decodeFromString<UserNullable>(jsonString2)
    println("User2: name=${user2.name}, age=${user2.age}")
    // name=Charlie, age=null (так как в классе стоит дефолт = null)

    // 3) Если бы поле age было не nullable (Int без ?),
    //    и при этом ключ отсутствовал или null — получите MissingFieldException или SerializationException.

    // Для наглядности — покажем, как можно создать объект JSON вручную:
    val dynamicJson = buildJsonObject {
        put("name", "Alice")
        // Можем не ставить age, чтобы проверить отсутствие
        // put("age", JsonNull) // это если хотим явно null
    }
    // Превращаем JsonObject в строку
    val dynamicString = dynamicJson.toString()
    val userDynamic = Json.decodeFromString<UserNullable>(dynamicString)
    println("UserDynamic: name=${userDynamic.name}, age=${userDynamic.age}")
}