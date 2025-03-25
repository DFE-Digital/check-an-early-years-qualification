output "redis_cache_id" {
  description = "ID of the Redis cache"
  value       = azurerm_redis_cache.redis.id
}

output "redis_cache_name" {
  description = "Name of the Redis cache instance"
  value       = azurerm_redis_cache.redis.name
}
