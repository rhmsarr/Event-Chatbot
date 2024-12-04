import constant  # Importing a module named 'constant' which likely contains constants like API keys.
import os  # Importing the os module for operating system-related functionality.
import sys  # Importing sys to access command-line arguments and system-related functionalities.
import cohere  # Importing the Cohere library for AI-based NLP tasks.
from langchain_core.vectorstores import InMemoryVectorStore  # Importing an in-memory vector store to handle vectorized data storage.
from langchain_core.documents import Document  # Importing Document to structure and manage text data with metadata.
from langchain_cohere import ChatCohere, CohereEmbeddings  # Importing Cohere's LLM and embedding functionalities.
from langchain_cohere import CohereRagRetriever  # Importing a retriever for retrieval-augmented generation using Cohere.

# Fetching the Cohere API key from a constants file.
COHERE_API_KEY = constant.API_KEY

# Initializing a Cohere language model (LLM) for conversational tasks.
llm = ChatCohere(cohere_api_key=COHERE_API_KEY,              
                 model="command-r-plus-08-2024",  # Specifying the model to use.
                 temperature=0)  # Setting temperature to 0 for deterministic responses.

# Initializing embeddings using Cohere for vectorizing text data.
embeddings = CohereEmbeddings(cohere_api_key=COHERE_API_KEY,
                              model="embed-english-v3.0")  # Specifying the embedding model.

# Creating an in-memory vector store to store vectorized representations of documents.
vector_store = InMemoryVectorStore(embeddings)

# Defining file paths for event and user data.
file_path = "data/events_data.txt"  # Path to the event data file.
file_path2 = "data/user_data.txt"  # Path to the user data file.

try:
    # Reading the content of the events data file.
    with open(file_path, "r", encoding="utf-8") as file:
        file_content = file.read()
    # Reading the content of the user data file.
    with open(file_path2, "r", encoding="utf-8") as file:
        file_content2 = file.read()

    # Creating documents with content and metadata for vectorization.
    documents = [
        Document(page_content=file_content, metadata={"source": file_path}),  # Event data as a document.
        Document(page_content=file_content2, metadata={"source": file_path2}),  # User data as a document.
    ]

    # Adding documents to the in-memory vector store for similarity search.
    vector_store.add_documents(documents=documents)
    print("Documents added successfully!")  # Confirmation message.

    # Retrieving the query from command-line arguments.
    query = sys.argv[1]  # First command-line argument is used as the query.
    print(f"User Input : {query}")  # Printing the user-provided query.

    # Performing similarity searches to retrieve relevant documents.
    results1 = vector_store.similarity_search(query, k=1)  # Top 1 most similar document.
    results2 = vector_store.similarity_search(query, k=2)  # Top 2 most similar documents.

    # Combining the results from both searches.
    combined = results1 + results2

    # Extracting the content of the retrieved documents.
    relevant_documents = [result.page_content for result in combined]
    # Constructing a detailed prompt for the LLM.
    prompt = (
        f"You are an intelligent assistant tasked with recommending events to users. "
        f"Based on the following documents and user preferences, recommend events "
        f"that align with their availability and interests. Address the user directly, "
        f"and format the explicate and interpreted response as bullet points.\n\n"
        f"### User Availability Information:\n"
        f"1. If 'AM' is true, the user is busy between 9 AM and 5 PM.\n"
        f"2. If 'PM' is true, the user is busy between 5 PM and 10 PM.\n"
        f"3. Events should be recommended for times when the user is not busy.\n\n"
        f"### Documents:\n{relevant_documents}\n\n"
        f"### Task:\n{query}"
    )

    # Invoking the LLM to generate a response based on the prompt.
    response = llm.invoke(prompt)
    print(f"Response: {response.content}")  # Printing the LLM-generated response.

# Handling any exceptions that may occur during file reading or processing.
except Exception as e:
    print(f"Document handling error: {e}")  # Printing the error message.
