# OrderingSystem
這是一個在面試中開發的簡單專案，實際上功能並不完整，例如缺少了帳號管理的部分。我主要是為了保留 Razor Pages 整合 Vue 2 的範例。

## Projects
 * [OrderingSystem.Web](./src/OrderingSystem.Web/)：  
 Razor Pages 網站。
 * [OrderingSystem.Infrastructure](./src/OrderingSystem.Infrastructure/)：  
 包含了一些基礎設施的實作，例如靜態工具、通用擴充方法等等。
 * [OrderingSystem.Domain](./src/OrderingSystem.Domain/)：  
 包含了業務邏輯的領域層實作。
 * [OrderingSystem.DataAccess](./src/OrderingSystem.DataAccess/)：  
 使用 Entity Framework Core 實作資料存取層的功能，包含 Entity Framework 的 DbContext、Entity 等實作。

## License
This project is MIT [licensed](./LICENSE.md).