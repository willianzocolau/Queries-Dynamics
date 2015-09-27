using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace System.Functions
{
    public class stcQuery
    {
        public string table;

        private StringBuilder csql;

        private ArrayList fields = new ArrayList();

        private ArrayList values = new ArrayList();

        private ArrayList join = new ArrayList();

        private ArrayList column_first = new ArrayList();

        private ArrayList column_second = new ArrayList();

        private ArrayList table_first = new ArrayList();

        private ArrayList table_second = new ArrayList();

        private ArrayList alias = new ArrayList();

        private ArrayList where = new ArrayList();

        private ArrayList complementary_select = new ArrayList();

        private ArrayList complementary_join = new ArrayList();

        private ArrayList complementary_where = new ArrayList();

        private ArrayList complementary_orderby = new ArrayList();

        private ArrayList complementary_groupby = new ArrayList();

        public enum Complementary_Type
        {
            Select,
            Join,
            Where,
            OrderBy,
            GroupBy
        };

        /// <summary>
        /// Constructor class.
        /// </summary>
        /// <param name="Table that will receive the query"></param>
        public stcQuery(string table)
        {
            this.table = table;
        }

        /// <summary>
        /// Method to add the fields of query.
        /// </summary>
        /// <param name="Name of field and value of field"></param>
        /// <param name="Value of field"></param>
        /// <example>
        /// stcQuery objQuery;
        /// objQuery = new stcQuery(stcTable.Tb_Table_1);
        /// objQuery.addField("column_1", column_1);
        /// </example>
        public void addField(string name, object value)
        {
            fields.Add(name);
            values.Add(value);
        }

        /// <summary>
        /// Method to add the joins in query.
        /// </summary>
        /// <param name="Type of join"></param>
        /// <param name="First table of join"></param>
        /// <param name="First column of join"></param>
        /// <param name="Second table of join"></param>
        /// <param name="Second column of join"></param>
        /// <example>
        /// stcQuery objQuery;
        /// objQuery = new stcQuery(stcTable.Tb_Table_1);
        /// objQuery.addField("column_1");
        /// objQuery.addField("column_2");
        /// objQuery.addJoin("inner join", stcTable.Tb_Table_2, "column_2", stcTable.Tb_Table_1, "column_1");
        /// </example>
        public void addJoin(string type_join, string fisrt_table, string first_column, string second_table, string second_column)
        {
            join.Add(type_join);
            table_first.Add(fisrt_table);
            table_second.Add(second_table);
            column_first.Add(first_column);
            column_second.Add(second_column);
        }

        /// <summary>
        /// Method to add the joins in query with alias.
        /// </summary>
        /// <param name="Type of join"></param>
        /// <param name="First table of join"></param>
        /// <param name="First column of join"></param>
        /// <param name="Second table of join"></param>
        /// <param name="Second column of join"></param>
        /// <param name="Alias table"></param>
        /// <example>
        /// stcQuery objQuery;
        /// objQuery = new stcQuery(stcTable.Tb_Table_1);
        /// objQuery.addField("column_1");
        /// objQuery.addField("column_2");
        /// objQuery.addJoin("inner join", stcTable.Tb_Table_2, "column_2", stcTable.Tb_Table_1, "column_1", "tb_2");
        /// </example>
        public void addJoin(string type_join, string fisrt_table, string first_column, string second_table, string second_column, string Alias)
        {
            join.Add(type_join);
            table_first.Add(fisrt_table);
            table_second.Add(second_table);
            column_first.Add(first_column);
            column_second.Add(second_column);
            alias.Add(Alias);
        }

        /// <summary>
        /// Method to add the fields of query (using for select).
        /// </summary>
        /// <param name="Name of field and value of field"></param>
        /// <example>
        /// stcQuery objQuery;
        /// objQuery = new stcQuery(stcTable.Tb_Table_1);
        /// objQuery.addField("column_1");
        /// </example>
        public void addField(string name)
        {
            addField(name, string.Empty);
        }

        /// <summary>
        /// Method to add the where in query.
        /// </summary>
        /// <param name="Name of field and value of field"></param>
        /// <param name="Value of field"></param>
        /// <example>
        /// stcQuery objQuery;
        /// objQuery = new stcQuery(stcTable.Tb_Table_1);
        /// objQuery.addField("column_1");
        /// objQuery.addWhere("column_1", column_1);
        /// </example>
        public void addWhere(string name, object value)
        {
            if (where.Count == 0)
            {
                where.Add("WHERE " + name + " = '" + value + "'");
                return;
            }
            else
            {
                where.Add(where[where.Count - 1] + " AND " + name + " = '" + value + "'");
            }
        }

        /// <summary>
        /// Method to add free instructions to query.
        /// </summary>
        /// <param name="Contents"></param>
        /// <param name="Complementary type"></param>
        /// <example>
        /// stcQuery objQuery;
        /// objQuery = new stcQuery(stcTable.Tb_Table_1);
        /// objQuery.addField("column_1");
        /// objQuery.addComplementary(" order by column_1 desc", stcQuery.Complementary_Type.OrderBy);
        /// </example>
        public void addComplementary(string contents, stcQuery.Complementary_Type type)
        {

            if (type == Complementary_Type.Select)
            {
                complementary_select.Add(contents);
            }
            else if (type == Complementary_Type.Join)
            {
                complementary_join.Add(contents);
            }
            else if (type == Complementary_Type.Where)
            {
                complementary_where.Add(contents);
            }
            else if (type == Complementary_Type.GroupBy)
            {
                complementary_groupby.Add(contents);
            }
            else if (type == Complementary_Type.OrderBy)
            {
                complementary_orderby.Add(contents);
            }
        }

        /// <summary>
        /// Method to construct insert queries.
        /// </summary>
        /// <example>
        /// stcQuery objQuery;
        /// objQuery = new stcQuery(stcTable.Tb_Table_1);
        /// objQuery.addField("column_1", column_1);
        /// objQuery.addField("column_2", column_2);
        /// string query = objQuery.Insert();
        /// </example>
        /// <returns>String with ready query.</returns>
        public string Insert()
        {
            csql = new StringBuilder();

            csql.Append("INSERT INTO ");

            csql.Append(table);

            csql.Append(" (");

            for (int i = 0; i < fields.Count; i++)
            {
                if (i == fields.Count - 1)
                {
                    csql.Append(fields[i]);
                }
                else
                {
                    csql.Append(fields[i] + ",");
                }
            }

            csql.Append(") VALUES(");

            for (int i = 0; i < values.Count; i++)
            {
                if (i == values.Count - 1)
                {
                    if (values[i] is DBNull)
                    {
                        csql.Append("NULL");
                    }
                    else if (values[i] is string)
                    {
                        csql.Append("'" + values[i] + "'");
                    }
                    else if (values[i] is DateTime)
                    {
                        csql.Append("'" + values[i] + "'");
                    }
                    else if (values[i] is TimeSpan)
                    {
                        csql.Append("'" + values[i] + "'");
                    }
                    else if (values[i] is decimal)
                    {
                        decimal value = 0;
                        decimal.TryParse(values[i].ToString(), out value);

                        csql.Append(value.ToString("G", CultureInfo.InvariantCulture));
                    }
                    else if (values[i] is double)
                    {
                        double value = 0;
                        double.TryParse(values[i].ToString(), out value);

                        csql.Append(value.ToString("G", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        csql.Append(values[i]);
                    }

                }
                else
                {
                    if (values[i] is DBNull)
                    {
                        csql.Append("NULL,");
                    }
                    else if (values[i] is string)
                    {
                        csql.Append("'" + values[i] + "'" + ",");
                    }
                    else if (values[i] is DateTime)
                    {
                        csql.Append("'" + values[i] + "'" + ",");
                    }
                    else if (values[i] is TimeSpan)
                    {
                        csql.Append("'" + values[i] + "'" + ",");
                    }
                    else if (values[i] is decimal)
                    {
                        decimal value = 0;
                        decimal.TryParse(values[i].ToString(), out value);

                        csql.Append(value.ToString("G", CultureInfo.InvariantCulture) + ",");
                    }
                    else if (values[i] is double)
                    {
                        double value = 0;
                        double.TryParse(values[i].ToString(), out value);

                        csql.Append(value.ToString("G", CultureInfo.InvariantCulture) + ",");
                    }
                    else
                    {
                        csql.Append(values[i] + ",");
                    }
                }
            }

            csql.Append(")");

            return csql.ToString();
        }

        /// <summary>
        /// Method to construct update queries.
        /// </summary>
        /// <param name="True or false - Parameter for security query."></param>
        /// <example>
        /// stcQuery objQuery;
        /// objQuery = new stcQuery(stcTable.Tb_Table_1);
        /// objQuery.addField("column_1", column_1);
        /// objQuery.addField("column_2", column_2);
        /// objQuery.addWhere("column_1", column_1);
        /// string query = objQuery.Update(false);
        /// </example>
        /// <returns>String with ready query.</returns>
        public string Update(bool no_where)
        {
            csql = new StringBuilder();

            csql.Append("UPDATE ");

            csql.Append(table);

            csql.Append(" SET ");

            for (int i = 0; i < fields.Count; i++)
            {
                csql.Append(fields[i] + " = ");

                if (values[i] is DBNull)
                {
                    csql.Append("NULL");
                }
                else if (values[i] is string)
                {
                    csql.Append("'" + values[i] + "'");
                }
                else if (values[i] is DateTime)
                {
                    csql.Append("'" + values[i] + "'");
                }
                else if (values[i] is TimeSpan)
                {
                    csql.Append("'" + values[i] + "'");
                }
                else if (values[i] is decimal)
                {
                    decimal value = 0;
                    decimal.TryParse(values[i].ToString(), out value);

                    csql.Append(value.ToString("G", CultureInfo.InvariantCulture));
                }
                else if (values[i] is double)
                {
                    double value = 0;
                    double.TryParse(values[i].ToString(), out value);

                    csql.Append(value.ToString("G", CultureInfo.InvariantCulture));
                }
                else
                {
                    csql.Append(values[i]);
                }

                if (i != fields.Count - 1)
                {
                    csql.Append(",");
                }
            }

            if (where.Count != 0)
            {
                csql.Append(" " + where[where.Count - 1]);
            }

            if (complementary_where.Count != 0)
            {
                for (int i = 0; i < complementary_where.Count; i++)
                {
                    csql.Append(" " + complementary_where[i]);
                }
            }

            if (where.Count == 0 && complementary_where.Count == 0 && no_where == false)
            {
                throw new Exception("instrução não aceita");
            }

            return csql.ToString();
        }

        /// <summary>
        /// Method to construct delete queries.
        /// </summary>
        /// <example>
        /// stcQuery objQuery;
        /// objQuery = new stcQuery(stcTable.Tb_Table_1);
        /// objQuery.addWhere("column_1", column_1);
        /// objQuery.addWhere("column_2", column_2);
        /// string query = objQuery.Delete();
        /// </example>
        /// <returns>String with ready query.</returns>
        public string Delete()
        {
            csql = new StringBuilder();

            csql.Append("DELETE FROM ");

            csql.Append(table);

            if (where.Count != 0)
            {
                csql.Append(" " + where[where.Count - 1]);
            }

            if (complementary_where.Count != 0)
            {
                for (int i = 0; i < complementary_where.Count; i++)
                {
                    csql.Append(" " + complementary_where[i]);
                }
            }

            return csql.ToString();
        }

        /// <summary>
        /// Method to construct select queries.
        /// </summary>
        /// <example>
        /// stcQuery objQuery;
        /// objQuery = new stcQuery(stcTable.Tb_Table_1);
        /// objQuery.addField("column_1");
        /// objQuery.addField("column_2");
        /// objQuery.addWhere("column_1", column_1);
        /// objQuery.addWhere("column_2", column_2);
        /// objQuery.addWhere("column_3", column_3);
        /// objQuery.addWhere("column_4", column_4);
        /// objQuery.addJoin("inner join", stcTable.Tb_Table_2, "column_2", stcTable.Tb_Table_1, "column_1", "tb_2");
        /// objQuery.addJoin("left join", stcTable.Tb_Table_3, "column_3", stcTable.Tb_Table_1, "column_1", "tb_3");
        /// objQuery.addComplementary(" order by column_1 desc", stcQuery.Complementary_Type.OrderBy);
        /// string query = objQuery.Select();
        /// </example>
        /// <returns>String with ready query.</returns>
        public string Select()
        {
            csql = new StringBuilder();

            csql.Append("SELECT ");

            for (int i = 0; i < fields.Count; i++)
            {
                if (i == fields.Count - 1)
                {
                    csql.Append(fields[i]);
                }
                else
                {
                    csql.Append(fields[i] + ",");
                }
            }

            if (complementary_select.Count != 0)
            {
                for (int i = 0; i < complementary_select.Count; i++)
                {
                    csql.Append(" " + complementary_select[i]);
                }
            }

            csql.Append(" FROM " + table);

            if (join.Count != 0)
            {
                for (int i = 0; i < join.Count; i++)
                {
                    if (alias.Count == 0)
                    {
                        csql.Append(" " + join[i] + " " + table_first[i] + " ON " + table_first[i] + "." + column_first[i] + " = " + table_second[i] + "." + column_second[i] + " ");
                    }
                    else if (alias[i] == null)
                    {
                        csql.Append(" " + join[i] + " " + table_first[i] + " ON " + table_first[i] + "." + column_first[i] + " = " + table_second[i] + "." + column_second[i] + " ");
                    }
                    else
                    {
                        csql.Append(" " + join[i] + " " + table_first[i] + " as " + alias[i] + " ON " + alias[i] + "." + column_first[i] + " = " + table_second[i] + "." + column_second[i] + " ");
                    }
                }
            }

            if (complementary_join.Count != 0)
            {
                for (int i = 0; i < complementary_join.Count; i++)
                {
                    csql.Append(" " + complementary_join[i]);
                }
            }

            if (where.Count != 0)
            {
                csql.Append(" " + where[where.Count - 1]);
            }

            if (complementary_where.Count != 0)
            {
                for (int i = 0; i < complementary_where.Count; i++)
                {
                    csql.Append(" " + complementary_where[i]);
                }
            }

            if (complementary_groupby.Count != 0)
            {
                for (int i = 0; i < complementary_groupby.Count; i++)
                {
                    csql.Append(" " + complementary_groupby[i]);
                }
            }

            if (complementary_orderby.Count != 0)
            {
                for (int i = 0; i < complementary_orderby.Count; i++)
                {
                    csql.Append(" " + complementary_orderby[i]);
                }
            }

            return csql.ToString();
        }
    }
}
