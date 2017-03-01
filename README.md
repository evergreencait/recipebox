### The get all method will return an empty list if the list of categories is empty in the beginning
    * input: {}
    * output: 0

### The equals method will return true if there are two categories that are the same
    * input: {"Mexican"}, {"Mexican"}
    * output: true

### The get all method will return the category if the category was saved in the database.
    * input: {"Mexican"}
    * output: "Mexican"

### The save method will assign a new id to an new instance of the category class.
    * input: {"Mexican", 0}
    * output: {"Mexican", non zero}

### The get all method will return a list of all categories.
    * input: {"Mexican"}, {"Thai"}
    * output: {"Mexican", "Thai"}

### The find method will return the category in the database.
    * input: {"Mexican"}
    * output: "Mexican"

### When the user updates the name of a category, the update method will return the updated name
    * input: {"Thai"}, {"Mexican"}
    * output: "Mexican"

### When the user deletes a category, the delete method will return an updated list without the deleted category
    * input: {"Mexican"}
    * output: {"American", "Thai Fusion", "Indian"}

### The get all method will return an empty list if the list of recipe is empty in the beginning
    * input: {}
    * output: ""

### The equals method will return true if there are two recipes that are the same
    * input: {"mac and cheese", "cheese and noodles", "cook it"}{"mac and cheese", "cheese and noodles", "cook it"}
    * output: true

### The save and get all methods will return true if the recipe was saved in the database
    * input: {"mac and cheese", "cheese and noodles", "cook it"}
    * output: true

### The get all method will return true if the id for the first recipe has an id of 1.
    input: {"mac and cheese", "cheese and noodles", "cook it"}
    output: 1

### The get all method will return a list of all recipes
    * input: {"mac and cheese", "cheese and noodles", "cook it"}, {"Chicken noodle soup", "chicken and water", "cook it"}
    * output: {"mac and cheese", "cheese and noodles", "cook it"}, {"Chicken noodle soup", "chicken and water", "cook it"}

### When the user updates the name or other property of a recipe, the update method will return the updated info
    * input: {"mac and cheese", "cheese and noodles and butter", "cook it"}, {"Chicken noodle soup", "chicken and water", "cook it"}
    * output:{"mac and cheese", "cheese and noodles and butter", "cook it"}, {"Chicken noodle soup", "chicken and water", "cook it"}

### When the user deletes a recipe, the delete method will return an updated list without the deleted recipe
    * input: {"mac and cheese", "cheese and noodles and butter", "cook it"}
    * output: {"Chicken noodle soup", "chicken and water", "cook it"}

### The get all method can sort recipes by rating
    * input: {"Chicken noodle soup", "chicken and water", "cook it"}{"mac and cheese", "cheese and noodles and butter", "cook it"}
    * output: {"mac and cheese", "cheese and noodles and butter", "cook it"}{"Chicken noodle soup", "chicken and water", "cook it"}
