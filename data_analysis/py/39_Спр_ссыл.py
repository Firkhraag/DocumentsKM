#!/usr/bin/env python
# coding: utf-8

# <h1>Ссылочные документы (+)</h1>

# <h1>Зависит от 40_Спр_ссыл_в</h1>

# In[1]:


import pandas as pd
import numpy as np
import pyodbc 

db_conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=LAPTOP-JSR6TV0G;'
                      'Database=Pro_t_londonSQL;'
                      'Trusted_Connection=yes;')


# In[2]:


df = pd.read_sql_query('SELECT * FROM Pro_t_londonSQL.dbo.Спр_ссыл', db_conn)
df.shape


# In[3]:


df.head()


# In[4]:


df = df.replace("", np.nan)


# In[5]:


df.isna().sum()


# <h2>Убираем</h2>

# In[6]:


df = df.dropna()
df.shape


# In[7]:


df = df.drop_duplicates(subset=["Шифр_сд"])
df.shape


# In[8]:


df = df.reset_index(drop=True)


# <h2>Trim whitespaces</h2>

# In[9]:


df["Шифр_сд"] = df["Шифр_сд"].str.strip()
df["Обозн_сд"] = df["Обозн_сд"].str.strip()
df["Наим_сд"] = df["Наим_сд"].str.strip()


# In[10]:


for i in range(0, len(df)):
    arr = df["Наим_сд"][i].split()
    new_str = ' '.join(arr)
    df.at[i, "Наим_сд"] = new_str


# <h2>Spelling</h2>

# In[11]:


df["Наим_сд"] = df["Наим_сд"].str.replace("возду ха", "воздуха")
df["Наим_сд"] = df["Наим_сд"].str.replace("Материриалы", "Материалы")
df["Наим_сд"] = df["Наим_сд"].str.replace("рекоментации", "рекомендации")
df["Наим_сд"] = df["Наим_сд"].str.replace("рас пашные", "распашные")
df["Наим_сд"] = df["Наим_сд"].str.replace("металличе ческих", "металлических")
df["Наим_сд"] = df["Наим_сд"].str.replace("металличе ских", "металлических")
df["Наим_сд"] = df["Наим_сд"].str.replace("электри ческие", "электрические")
df["Наим_сд"] = df["Наим_сд"].str.replace("кра ны", "краны")
df["Наим_сд"] = df["Наим_сд"].str.replace("примене нием", "применением")
df["Наим_сд"] = df["Наим_сд"].str.replace("обслуживани сосудов", "обслуживания сосудов")
df["Наим_сд"] = df["Наим_сд"].str.replace("пе реплетами", "переплетами")
df["Наим_сд"] = df["Наим_сд"].str.replace("за полнением", "заполнением")
df["Наим_сд"] = df["Наим_сд"].str.replace("линй", "линий")
df["Наим_сд"] = df["Наим_сд"].str.replace("мосто вые", "мостовые")
df["Наим_сд"] = df["Наим_сд"].str.replace("покрыти ем", "покрытием")
df["Наим_сд"] = df["Наим_сд"].str.replace("со кращенному", "сокращенному")
df["Наим_сд"] = df["Наим_сд"].str.replace("покрытияпролетами", "покрытия пролетами")
df["Наим_сд"] = df["Наим_сд"].str.replace("широкополочныых", "широкополочных")
df["Наим_сд"] = df["Наим_сд"].str.replace("тав ров", "тавров")
df["Наим_сд"] = df["Наим_сд"].str.replace("зда ний", "зданий")
df["Наим_сд"] = df["Наим_сд"].str.replace("ремонтно -монтажных", "ремонтно-монтажных")
df["Наим_сд"] = df["Наим_сд"].str.replace("пок рытия", "покрытия")
df["Наим_сд"] = df["Наим_сд"].str.replace("профилирован ным", "профилированным")
df["Наим_сд"] = df["Наим_сд"].str.replace("стеклои минераловатных", "стекло и минераловатных")
df["Наим_сд"] = df["Наим_сд"].str.replace("окон,дверей", "окон, дверей")
df["Наим_сд"] = df["Наим_сд"].str.replace("пром.зданий", "пром. зданий")
df["Наим_сд"] = df["Наим_сд"].str.replace("строитель ные", "строительные")
df["Наим_сд"] = df["Наим_сд"].str.replace("сталь ные", "стальные")
df["Наим_сд"] = df["Наим_сд"].str.replace("наруж ного", "наружного")
df["Наим_сд"] = df["Наим_сд"].str.replace("ветров.район.", "ветров. район.")
df["Наим_сд"] = df["Наим_сд"].str.replace("выше и ниже -40 град.с.  ше и ниже -40 град.с.", "выше -40 град.с. и ниже -40 град.с.")
df["Наим_сд"] = df["Наим_сд"].str.replace("алю миниевых", "алюминиевых")
df["Наим_сд"] = df["Наим_сд"].str.replace("зданий,промышленных", "зданий, промышленных")
df["Наим_сд"] = df["Наим_сд"].str.replace("предприятий.шарнирные", "предприятий. Шарнирные")
df["Наим_сд"] = df["Наим_сд"].str.replace("связей.Чертежи", "связей. Чертежи")
df["Наим_сд"] = df["Наим_сд"].str.replace("сталь ными", "стальными")
df["Наим_сд"] = df["Наим_сд"].str.replace("унифици рованных", "унифицированных")
df["Наим_сд"] = df["Наим_сд"].str.replace("уз лы", "узлы")
df["Наим_сд"] = df["Наим_сд"].str.replace("железобе тонных", "железобетонных")
df["Наим_сд"] = df["Наим_сд"].str.replace("метал лопроката", "металлопроката")
df["Наим_сд"] = df["Наим_сд"].str.replace("выпу ску", "выпуску")
df["Наим_сд"] = df["Наим_сд"].str.replace("откры вающиеся", "открывающиеся")
df["Наим_сд"] = df["Наим_сд"].str.replace("светоаэра ционных", "светоаэрационных")
df["Наим_сд"] = df["Наим_сд"].str.replace("проектирования.Рабочие", "проектирования. Рабочие")
df["Наим_сд"] = df["Наим_сд"].str.replace("черте жи", "чертежи")
df["Наим_сд"] = df["Наим_сд"].str.replace("опорн.кранами", "опорн. кранами")
df["Наим_сд"] = df["Наим_сд"].str.replace("опорынми кранами", "опорными кранами")
df["Наим_сд"] = df["Наим_сд"].str.replace("несущ.конструкций", "несущ. конструкций")
df["Наим_сд"] = df["Наим_сд"].str.replace("подв.кранами", "подв. кранами")
df["Наим_сд"] = df["Наим_сд"].str.replace("кровлей,оборудованных", "кровлей, оборудованных")
df["Наим_сд"] = df["Наим_сд"].str.replace("одноэт.произв", "одноэт. произв")
df["Наим_сд"] = df["Наим_сд"].str.replace("произв.зданий", "произв. зданий")
df["Наим_сд"] = df["Наим_сд"].str.replace("кровлей,оборудованных", "кровлей, оборудованных")
df["Наим_сд"] = df["Наим_сд"].str.replace("специальн.назначения", "специальн. назначения")
df["Наим_сд"] = df["Наим_сд"].str.replace("зданий,оборудованных", "зданий, оборудованных")
df["Наим_сд"] = df["Наим_сд"].str.replace("две рей", "дверей")


# <h2>Id</h2>

# In[12]:


df.insert(0, 'id', range(1, len(df) + 1))


# <h2>Rename columns</h2>

# In[13]:


df = df.rename(columns={"Шифр_сд": "code",
                        "Вид_сд": "type_id",
                        "Обозн_сд": "designation",
                        "Наим_сд": "name"
                       })
df


# In[14]:


df["name"].value_counts()


# In[15]:


df = df.drop_duplicates(subset=["name"])


# In[16]:


df["name"].value_counts()


# In[17]:


df.loc[df["name"].str.endswith('.') == False, "name"] = df[df["name"].str.endswith('.') == False]["name"] + '.'


# In[18]:


df.loc[df["name"].str.endswith('.') == False, "name"]


# <h1>Postgres</h1>

# In[19]:


from psycopg2 import connect, sql, DatabaseError
import psycopg2.extras as extras


# DocumentsKM

# In[20]:


# Connect
try:
    conn = connect (
        dbname = "documentskm",
        user = "postgres",
        host = "localhost",
        password = "password"
    )
    cursor = conn.cursor()
except Exception as err:
    cursor = None
    print("Psycopg2 error:", err)
    
# Check if the connection was valid
if cursor != None:
    print("Connection successful")


# In[21]:


cursor.execute(open("sql/39.sql", "r").read())
conn.commit()


# <h2>Insert data</h2>

# In[22]:


def execute_values(conn, df, table):
    tuples = [tuple(x) for x in df.to_numpy()]
    cols = ','.join(list(df.columns))
    query  = "INSERT INTO %s(%s) VALUES %%s" % (table, cols)
    cursor = conn.cursor()
    try:
        extras.execute_values(cursor, query, tuples)
        conn.commit()
    except (Exception, DatabaseError) as error:
        print("Error: %s" % error)
        conn.rollback()
        cursor.close()
        return 1
    print("execute_values() done")
    cursor.close()


# In[23]:


execute_values(conn, df, "linked_docs")


# In[ ]:




