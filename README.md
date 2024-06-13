# Talabat API

## Overview
Talabat API is an E-commerce website built using .Net Core 8. This platform provides a seamless user experience, allowing customers to browse different types of products, create orders, choose delivery methods, and confirm orders. For payment processing, the Stripe service is integrated to ensure secure and efficient transactions.

## Features
- **Restful API:** Provides a robust and scalable way to handle HTTP requests and responses.
- **C#:** The primary programming language used for developing the application.
- **Entity Framework:** Used for data access, handling database operations through LINQ.
- **LINQ:** Language Integrated Query, used for querying and manipulating data.
- **Repository Design Pattern:** Used to create an abstraction layer between the data access layer and the business logic layer of the application.
- **Redis:** Utilized as an in-memory data store for caching and improving application performance.
- **Stripe Service:** Integrated for handling secure online payments.

## User Experience
1. **Product Selection:** Users can browse and choose from different types of products.
2. **Order Creation:** Users can create their orders by selecting desired products.
3. **Delivery Method:** Users can choose their preferred delivery method.
4. **Basket Confirmation:** Users can review their basket, confirm their order, and proceed to payment.
5. **Payment:** Users can pay with their cards through the Stripe service.

## Getting Started
### Prerequisites
- .NET Core 8 SDK
- SQL Server or any other supported database
- Redis
- Stripe account and API keys

### Installation
1. **Clone the repository:**
   ```bash
   git clone https://github.com/Talabat-E-commerce/talabat-api.git
   cd talabat-api
