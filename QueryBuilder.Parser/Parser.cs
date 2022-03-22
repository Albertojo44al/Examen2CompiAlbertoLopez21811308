using System;
using QueryBuilder.Core.Enums;
using QueryBuilder.Core.Interfaces;
using QueryBuilder.Core.Models;
using QueryBuilder.Core.Expressions;
using QueryBuilder.Core.Statements;
using QueryBuilder.Core;
using System.Collections.Generic;

namespace QueryBuilder.Parser
{
    public class Parser : IParser
    {
        private readonly IScanner _scanner;
        private readonly ILogger _logger;
        private Token _lookAhead;
        private string tableName;
        public Parser(IScanner scanner, ILogger logger)
        {
            this._scanner = scanner;
            this._logger = logger;
            this._lookAhead = this._scanner.GetNextToken();
        }

        public Statement Parse()
        {
            return Code();
        }

        private Statement Code()
        {
            Match(TokenType.DefKeyword);
            Match(TokenType.TablesKeyword);
            TableDef();
            TableDefs();
            Match(TokenType.EndKeyWord);
            Match(TokenType.DefKeyword);
            Match(TokenType.RelationshipsKeyword);
            Relationships();
            Match(TokenType.EndKeyWord);
            return new CodeStatement(Queries());
        }

        private void Relationships()
        {
            if (_lookAhead == TokenType.Identifier)
            {
                Relationship();
                Relationships();
            }
        }

        private void Relationship()
        {
            Match(TokenType.Identifier);
            Match(TokenType.Dot);
            if (_lookAhead == TokenType.ManyKeyword)
            {
                Match(TokenType.ManyKeyword);
            }
            else
            {
                Match(TokenType.OneKeyword);
            }

            Match(TokenType.LeftParens);
            Match(TokenType.Identifier);
            Match(TokenType.RightParens);
            Match(TokenType.Semicolon);
        }

        private Statement Queries()
        {
            if (_lookAhead != TokenType.Identifier)
            {
                return null;
            }
            return new SequenceQueriesStatement(Query(), Queries());

        }

        private Statement Query()
        {
            var token = _lookAhead;
            tableName = token.Lexeme;
            Match(TokenType.Identifier);
            Match(TokenType.Dot);
            switch (_lookAhead.TokenType)
            {
                case TokenType.AddKeyword:
                    var properties = ContextTable.GetAllProperties(token.Lexeme);
                    return new AddStatement(properties, Insert()); ;
                case TokenType.UpdateKeyword:
                    properties = ContextTable.GetAllProperties(token.Lexeme);
                    Match(TokenType.UpdateKeyword);
                    Match(TokenType.LeftParens);
                    var json = Json();
                    Match(TokenType.RightParens);
                    var expression = Update();
                    return new UpdateStatement(json, properties, expression);
                default:
                    properties = ContextTable.GetAllProperties(token.Lexeme);
                    Match(TokenType.SelectKeyword);
                    Match(TokenType.LeftParens);
                    var args = Args();
                    Match(TokenType.RightParens);
                    expression = Select();
                    return new SelectStatement(args, properties, expression);
            }
        }

        private Expression Select()
        {
            if (_lookAhead == TokenType.Semicolon)
            {
                Match(TokenType.Semicolon);
                return null;
            }
            else
            {
                Match(TokenType.Dot);
                Match(TokenType.WhereKeyword);
                Match(TokenType.LeftParens);
                var expression = LogicalOrExpr();
                Match(TokenType.RightParens);
                Match(TokenType.Semicolon);
                return expression;
            }
        }

        private IEnumerable<Expression> Args()
        {
            var args = new List<Expression>();
            args.Add(LogicalOrExpr());
            args.AddRange(ArgsPrime());
            return args;
        }

        private IEnumerable<Expression> ArgsPrime()
        {
            var args = new List<Expression>();
            if (_lookAhead == TokenType.Comma)
            {
                Match(TokenType.Comma);
                args.Add(LogicalOrExpr());
                args.AddRange(ArgsPrime());
            }
            return args;
        }


        private Expression Update()
        {
            if (_lookAhead == TokenType.Semicolon)
            {
                Match(TokenType.Semicolon);
                return null;
            }
            else
            {
                Match(TokenType.Dot);
                Match(TokenType.WhereKeyword);
                Match(TokenType.LeftParens);
                var expression = LogicalOrExpr();
                Match(TokenType.RightParens);
                Match(TokenType.Semicolon);
                return expression;
            }
        }

        private IEnumerable<Expression> Json()
        {
            var elements = new List<Expression>();
            Match(TokenType.LeftBrace);
            elements.AddRange(JsonElementsOptional());
            Match(TokenType.RightBrace);

            return elements;
        }

        private IEnumerable<Expression> JsonElementsOptional()
        {
            if (_lookAhead == TokenType.Identifier)
            {
                return JsonElements();
            }
            return null;
        }

        private IEnumerable<Expression> JsonElements()
        {
            var elements = new List<Expression>();
            elements.Add(JsonElementBlock());
            while (_lookAhead == TokenType.Comma)
            {
                Match(TokenType.Comma);
                elements.Add(JsonElementBlock());
            }
            return elements;
        }

        private Expression JsonElementBlock()
        {
            Match(TokenType.Identifier);
            Match(TokenType.Colon);
            return LogicalOrExpr();
        }

        private IEnumerable<Expression> Insert()
        {
            var elements = new List<Expression>();
            Match(TokenType.AddKeyword);
            Match(TokenType.LeftParens);
            elements.AddRange(Json());
            Match(TokenType.RightParens);
            Match(TokenType.Semicolon);

            return elements;
        }

        private void TableDefs()
        {
            if (_lookAhead == TokenType.Identifier)
            {
                TableDef();
                TableDefs();
            }
        }

        private void TableDef()
        {
            var token = this._lookAhead;
            Match(TokenType.Identifier);
            Match(TokenType.LeftBrace);
            var args = new List<IdExpression>();
            args.AddRange(TableColumns());
            Match(TokenType.RightBrace);
            ContextTable.Add(token.Lexeme, args);

        }

        private IEnumerable<IdExpression> TableColumns()
        {
            var args = new List<IdExpression>();
            while (_lookAhead == TokenType.LeftBracket || _lookAhead == TokenType.Identifier)
            {
                switch (_lookAhead)
                {
                    case { TokenType: TokenType.LeftBracket }:
                        Match(TokenType.LeftBracket);
                        Match(TokenType.PrimaryKeyword);
                        Match(TokenType.RightBracket);
                        var token = this._lookAhead;
                        Match(TokenType.Identifier);
                        Match(TokenType.Colon);
                        var type = Type();
                        var id = new IdExpression(token, type);
                        args.Add(id);
                        Match(TokenType.Semicolon);
                        break;
                    default:
                        token = this._lookAhead;
                        Match(TokenType.Identifier);
                        Match(TokenType.Colon);
                        type = Type();
                        id = new IdExpression(token, type);
                        args.Add(id);
                        Match(TokenType.Semicolon);
                        break;
                }

            }
            return args;
        }
        private CompilerType Type()
        {
            switch (_lookAhead)
            {
                case {TokenType: TokenType.IntKeyword}:
                    Match(TokenType.IntKeyword);
                    return CompilerType.Int;
                case {TokenType: TokenType.FloatKeyword}:
                    Match(TokenType.FloatKeyword);
                    return CompilerType.Float;
                case {TokenType: TokenType.BoolKeyword}:
                    Match(TokenType.BoolKeyword);
                    return CompilerType.Bool;
                default:
                    Match(TokenType.StringKeyword);
                    return CompilerType.String;
            }
        }

        private Expression LogicalOrExpr()
        {
            var expression = LogicalAndExpr();
            while (this._lookAhead.TokenType == TokenType.LogicalOr)
            {
                var token = this._lookAhead;
                this.Move();
                expression = new LogicalExpression(token, expression, LogicalAndExpr());

            }
            return expression;
        }

        private Expression LogicalAndExpr()
        {
            var expression = Eq();
            while (this._lookAhead.TokenType == TokenType.LogicalAnd)
            {
                var token = this._lookAhead;
                this.Move();
                expression = new LogicalExpression(token, expression,  Eq());
            }
            return expression;
        }

        private Expression Eq()
        {
            var expression = Rel();
            while (this._lookAhead.TokenType == TokenType.Equal)
            {
                var token = this._lookAhead;
                this.Move();
                expression = new LogicalExpression(token, expression, Rel());
            }
            return expression;
        }


        private Expression Rel()
        {
            var expression = Expr();
            while (this._lookAhead.TokenType == TokenType.LessThan ||
                   this._lookAhead.TokenType == TokenType.GreaterThan ||
                   this._lookAhead.TokenType == TokenType.LessOrEqualThan ||
                   this._lookAhead.TokenType == TokenType.GreaterOrEqualThan)
            {
                var token = this._lookAhead;
                this.Move();
                expression = new RelationalExpression(token, expression, Expr());
            }

            return expression;
        }

        private Expression Expr()
        {
            var expression = Term();
            while (this._lookAhead.TokenType == TokenType.Plus || this._lookAhead.TokenType == TokenType.Minus)
            {
                var token = this._lookAhead;
                this.Move();
                expression = new ArithmeticExpression(token, expression, Term());
            }

            return expression;
        }

        private Expression Term()
        {
            var expression = Factor();
            while (this._lookAhead.TokenType == TokenType.Multiplication ||
                   this._lookAhead.TokenType == TokenType.Division)
            {
                var token = this._lookAhead;
                this.Move();
                expression = new ArithmeticExpression(token, expression, Factor());
            }
            return expression;
        }

        private Expression Factor()
        {
            switch (this._lookAhead.TokenType)
            {
                case TokenType.LeftParens:
                    this.Match(TokenType.LeftParens);
                    var expression  = LogicalOrExpr();
                    this.Match(TokenType.RightParens);
                    return expression;
                case TokenType.Identifier:
                    var token = this._lookAhead;
                    Match(TokenType.Identifier);
                    return ContextTable.GetProperty(tableName, token.Lexeme);
                case TokenType.IntConstant:
                    token = this._lookAhead;
                    this.Match(TokenType.IntConstant);
                    return new ConstantExpression(token, Core.CompilerType.Int);
                case TokenType.FloatConstant:
                    token = this._lookAhead;
                    this.Match(TokenType.FloatConstant);
                    return new ConstantExpression(token, Core.CompilerType.Float);
                case TokenType.TrueKeyword:
                    token = this._lookAhead;
                    this.Match(TokenType.TrueKeyword);
                    return new ConstantExpression(token, Core.CompilerType.Bool);
                case TokenType.FalseKeyword:
                    token = this._lookAhead;
                    this.Match(TokenType.FalseKeyword);
                    return new ConstantExpression(token, Core.CompilerType.Bool);
                default:
                    token = this._lookAhead;
                    this.Match(TokenType.StringLiteral);
                    return new ConstantExpression(token, Core.CompilerType.String);
            }
        }

        private void Move()
        {
            this._lookAhead = this._scanner.GetNextToken();
        }

        private void Match(TokenType expectedTokenType)
        {
            if (this._lookAhead != expectedTokenType)
            {
                this._logger.Error(
                    $"Syntax Error! expected token {expectedTokenType} but found {this._lookAhead.TokenType} on line {this._lookAhead.Line} and column {this._lookAhead.Column}");
                throw new ApplicationException(
                    $"Syntax Error! expected token {expectedTokenType} but found {this._lookAhead.TokenType} on line {this._lookAhead.Line} and column {this._lookAhead.Column}");
            }

            this.Move();
        }
    }
}