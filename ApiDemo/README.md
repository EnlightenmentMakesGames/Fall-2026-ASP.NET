ApiDemo showcases a basic fetch API to retrieve the titles of the five most recently added blog posts.

New posts can be added by inputing the following command while the demo is running:
`curl -X POST `(host address)`/api/posts -H "Content-Type: application/json" -d "{\"title\":\"`(post title)`\",\"body\":\"`[post body]`\"}"`

Dashboard page:
!(Dashboard Page)[Dashboard.PNG]
