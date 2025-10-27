#  TestTask-3 — Inventory System for Unity

##  Описание
**TestTask-3** — это модульная система инвентаря, разработанная для демонстрации архитектуры, логики и гибкости в Unity.  
Она реализует **MVC-подход (Model-View-Controller)** и позволяет игроку управлять предметами в инвентаре:  
перетаскивать, использовать, выбрасывать, сортировать и сохранять своё состояние между сессиями.

---

##  Основные возможности

 Фиксированное количество слотов (например, 5×4)  
 Несколько типов предметов (оружие, зелья, квест-предметы и т.д.)  
 Стекуемые / не стекуемые предметы  
 Drag & Drop между слотами  
 Использование (Use) и выбрасывание (Drop) предметов  
 Подсказка (Tooltip) при наведении на предмет  
 Сортировка по имени, типу и направлению (по возрастанию / убыванию)  
 Сохранение и загрузка инвентаря (JSON-файл)  
 Готовая система расширения (через интерфейсы и ScriptableObject)

---

##  Архитектура проекта

###  Model (Данные)
- **InventoryItem** — класс, хранящий ссылку на `ItemDefinition` и количество (`quantity`).  
- **ItemDefinition (ScriptableObject)** — шаблон предмета с полями:
  - `itemName`
  - `description`
  - `icon`
  - `itemType`
  - `isStackable`
  - `maxStack`

###  Core (Логика)
- **Inventory** — управляет внутренним списком предметов:
  - добавление (`AddItem`, `AddItemDirect`)
  - удаление (`RemoveItemAtSlot`)
  - стекование (`TryStackItems`)
  - сортировка и уведомление UI (`OnInventoryChanged`)

- **InventorySave** — отвечает за сохранение и загрузку:
  - сохраняет все предметы в `inventory.json`
  - при запуске восстанавливает состояние инвентаря

###  Controller
- **InventoryController** — посредник между логикой и UI.  
  Обрабатывает действия игрока:
  - использование (`OnUseItem`)
  - выбрасывание (`OnDropItemAtWorld`)
  - перестановку (`OnSwapOrStack`)
  - вызывает обновление UI через событие `OnInventoryChanged`

###  View (UI)
- **InventoryUI** — управляет отображением всех слотов и их обновлением.  
- **InventorySlot** — отдельный слот:
  - поддерживает Drag & Drop  
  - показывает иконку, количество  
  - имеет кнопки **Use** и **Drop**  
- **InventoryTooltip** — всплывающее окно при наведении (название, описание).  
- **ItemPreviewUI** — отдельное окно с подробным описанием предмета.  
- **InventorySortUI** — кнопки сортировки (по имени, по типу, смена направления).

---

##  Как запустить проект

### 1. Открой сцену
- Открой проект в **Unity 2022.3.62f2**.  
- Загрузи сцену `MainScene.unity`.

### 2. Добавь объекты на сцену
На сцене должны быть:
- `InventoryManager` (ссылки на `ItemDefinitions`)
- `InventoryController`
- `InventoryUI` (Canvas с сеткой слотов)
- `InventoryTooltip` (префаб с CanvasGroup, TMP Text)
- `ItemPreviewUI` (окно с информацией о предмете)
- `InventorySortUI` (панель с кнопками сортировки)

### 3. Настрой связи
- В **Bootstrap** укажи:
  _inventoryManager
  _inventoryController
  _inventoryUI
В InventoryUI укажи префаб слота и родительский Transform для размещения ячеек.

В InventoryManager добавь список ItemDefinition (Health Potion, Sword и т.д.)

4. Настрой предметы
Создай в Unity:

Assets/InventorySystem/ScriptableObjects/HealthPotion.asset
Health Potion:

Поле	Значение
Item Name	Health Potion
Description	Восстанавливает 50 HP при использовании.
Item Type	Consumable
Icon	(спрайт зелья)
Is Stackable
Max Stack	99

5. Запусти сцену (Play)
Перетаскивай предметы между слотами.

Наведи курсор — появится Tooltip.

Нажми Use — предмет применяется.

Нажми Drop — теряется 1 штука.

Нажми Sort By Name / Sort By Type — сортировка предметов.

Нажми Toggle Order — смена направления сортировки.

 Система сохранения
Сохраняется автоматически при любом изменении инвентаря.
Файл находится по пути:

php-template
Копировать код
C:\Users\<User>\AppData\LocalLow\<CompanyName>\<ProjectName>\inventory.json
или в Unity через Application.persistentDataPath.

Загружается при запуске через InventorySaveSystem.LoadInventory().

 На что обратить внимание
Архитектура: чёткое разделение Model / View / Controller.

События: обновление UI только через OnInventoryChanged.

Сортировка: корректно обновляет внутренний список без дубликатов.

Интерфейсы: IInventoryManager позволяет легко подменять реализацию (например, сетевой инвентарь).

Сохранение: JSON-сериализация, независимая от сцены.

 Пример взаимодействия компонентов
Player Action → InventorySlot → InventoryController → InventoryManager
                                          ↓
                                    InventoryUI.Refresh
                                          ↓
                                    InventoryTooltip / ItemPreview
