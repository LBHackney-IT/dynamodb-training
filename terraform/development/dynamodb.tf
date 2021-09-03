resource "aws_dynamodb_table" "trainingapi_dynamodb_table" {
    name                  = "dynamodb-training"
    billing_mode          = "PROVISIONED"
    read_capacity         = 10
    write_capacity        = 10
    hash_key              = "id"

    attribute {
        name              = "id"
        type              = "S"
    }

    tags = {
        Name              = "dynamodb-training-api-${var.environment_name}"
        Environment       = var.environment_name
        terraform-managed = true
        project_name      = var.project_name
        backup_policy     = "Dev"
    }

    point_in_time_recovery {
        enabled           = true
    }
}