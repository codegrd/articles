import kotlinx.serialization.Serializable
import kotlinx.serialization.json.Json

@Serializable
data class Details(
    val email: String,
    val address: String
)

@Serializable
data class Profile(
    val name: String,
    val details: Details
)

@Serializable
data class UserData(
    val id: Int,
    val profile: Profile
)

@Serializable
data class ApiResponse(
    val user: UserData
)

fun runNestedStructuresExample() {
    val jsonString = """
        {
          "user": {
            "id": 123,
            "profile": {
              "name": "Alice",
              "details": {
                "email": "alice@example.com",
                "address": "Some Street"
              }
            }
          }
        }
    """.trimIndent()

    val response = Json.decodeFromString<ApiResponse>(jsonString)
    println("User ID: ${response.user.id}")
    println("Name: ${response.user.profile.name}")
    println("Email: ${response.user.profile.details.email}")
}

fun main() {
    runNestedStructuresExample()
}