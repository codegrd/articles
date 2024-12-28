plugins {
    // Основной плагин для Kotlin (JVM)
    kotlin("jvm") version "2.0.20"

    // Плагин для поддержки Kotlin Serialization
    id("org.jetbrains.kotlin.plugin.serialization") version "1.8.10"
}

group = "org.example"
version = "1.0-SNAPSHOT"

repositories {
    mavenCentral()
}

dependencies {
    // Библиотека, которая позволит использовать @Serializable, Json.decodeFromString и т.д.
    implementation("org.jetbrains.kotlinx:kotlinx-serialization-json:1.5.1")

    // Зависимости для тестов
    testImplementation(kotlin("test"))
}

tasks.test {
    useJUnitPlatform()
}
kotlin {
    jvmToolchain(21)
}