﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
#region EDM Relationship Metadata

[assembly: EdmRelationshipAttribute("HabraStatsModel", "PostComments", "Post", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(HabrApi.EntityModel.Post), "Comment", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(HabrApi.EntityModel.Comment), true)]

#endregion

namespace HabrApi.EntityModel
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class HabraStatsEntities1 : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new HabraStatsEntities1 object using the connection string found in the 'HabraStatsEntities1' section of the application configuration file.
        /// </summary>
        public HabraStatsEntities1() : base("name=HabraStatsEntities1", "HabraStatsEntities1")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new HabraStatsEntities1 object.
        /// </summary>
        public HabraStatsEntities1(string connectionString) : base(connectionString, "HabraStatsEntities1")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new HabraStatsEntities1 object.
        /// </summary>
        public HabraStatsEntities1(EntityConnection connection) : base(connection, "HabraStatsEntities1")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Comment> Comments
        {
            get
            {
                if ((_Comments == null))
                {
                    _Comments = base.CreateObjectSet<Comment>("Comments");
                }
                return _Comments;
            }
        }
        private ObjectSet<Comment> _Comments;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Post> Posts
        {
            get
            {
                if ((_Posts == null))
                {
                    _Posts = base.CreateObjectSet<Post>("Posts");
                }
                return _Posts;
            }
        }
        private ObjectSet<Post> _Posts;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Comments EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToComments(Comment comment)
        {
            base.AddObject("Comments", comment);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Posts EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToPosts(Post post)
        {
            base.AddObject("Posts", post);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="HabraStatsModel", Name="Comment")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Comment : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Comment object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="text">Initial value of the Text property.</param>
        /// <param name="url">Initial value of the Url property.</param>
        /// <param name="postId">Initial value of the PostId property.</param>
        /// <param name="postUrl">Initial value of the PostUrl property.</param>
        /// <param name="postTitle">Initial value of the PostTitle property.</param>
        /// <param name="score">Initial value of the Score property.</param>
        /// <param name="userName">Initial value of the UserName property.</param>
        /// <param name="avatar">Initial value of the Avatar property.</param>
        public static Comment CreateComment(global::System.String id, global::System.String text, global::System.String url, global::System.Int32 postId, global::System.String postUrl, global::System.String postTitle, global::System.Int32 score, global::System.String userName, global::System.String avatar)
        {
            Comment comment = new Comment();
            comment.Id = id;
            comment.Text = text;
            comment.Url = url;
            comment.PostId = postId;
            comment.PostUrl = postUrl;
            comment.PostTitle = postTitle;
            comment.Score = score;
            comment.UserName = userName;
            comment.Avatar = avatar;
            return comment;
        }

        #endregion

        #region Simple Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value, false, "Id");
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.String _Id;
        partial void OnIdChanging(global::System.String value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Text
        {
            get
            {
                return _Text;
            }
            set
            {
                OnTextChanging(value);
                ReportPropertyChanging("Text");
                _Text = StructuralObject.SetValidValue(value, false, "Text");
                ReportPropertyChanged("Text");
                OnTextChanged();
            }
        }
        private global::System.String _Text;
        partial void OnTextChanging(global::System.String value);
        partial void OnTextChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Url
        {
            get
            {
                return _Url;
            }
            set
            {
                OnUrlChanging(value);
                ReportPropertyChanging("Url");
                _Url = StructuralObject.SetValidValue(value, false, "Url");
                ReportPropertyChanged("Url");
                OnUrlChanged();
            }
        }
        private global::System.String _Url;
        partial void OnUrlChanging(global::System.String value);
        partial void OnUrlChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 PostId
        {
            get
            {
                return _PostId;
            }
            set
            {
                OnPostIdChanging(value);
                ReportPropertyChanging("PostId");
                _PostId = StructuralObject.SetValidValue(value, "PostId");
                ReportPropertyChanged("PostId");
                OnPostIdChanged();
            }
        }
        private global::System.Int32 _PostId;
        partial void OnPostIdChanging(global::System.Int32 value);
        partial void OnPostIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String PostUrl
        {
            get
            {
                return _PostUrl;
            }
            set
            {
                OnPostUrlChanging(value);
                ReportPropertyChanging("PostUrl");
                _PostUrl = StructuralObject.SetValidValue(value, false, "PostUrl");
                ReportPropertyChanged("PostUrl");
                OnPostUrlChanged();
            }
        }
        private global::System.String _PostUrl;
        partial void OnPostUrlChanging(global::System.String value);
        partial void OnPostUrlChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String PostTitle
        {
            get
            {
                return _PostTitle;
            }
            set
            {
                OnPostTitleChanging(value);
                ReportPropertyChanging("PostTitle");
                _PostTitle = StructuralObject.SetValidValue(value, false, "PostTitle");
                ReportPropertyChanged("PostTitle");
                OnPostTitleChanged();
            }
        }
        private global::System.String _PostTitle;
        partial void OnPostTitleChanging(global::System.String value);
        partial void OnPostTitleChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> Date
        {
            get
            {
                return _Date;
            }
            set
            {
                OnDateChanging(value);
                ReportPropertyChanging("Date");
                _Date = StructuralObject.SetValidValue(value, "Date");
                ReportPropertyChanged("Date");
                OnDateChanged();
            }
        }
        private Nullable<global::System.DateTime> _Date;
        partial void OnDateChanging(Nullable<global::System.DateTime> value);
        partial void OnDateChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Score
        {
            get
            {
                return _Score;
            }
            set
            {
                OnScoreChanging(value);
                ReportPropertyChanging("Score");
                _Score = StructuralObject.SetValidValue(value, "Score");
                ReportPropertyChanged("Score");
                OnScoreChanged();
            }
        }
        private global::System.Int32 _Score;
        partial void OnScoreChanging(global::System.Int32 value);
        partial void OnScoreChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                OnUserNameChanging(value);
                ReportPropertyChanging("UserName");
                _UserName = StructuralObject.SetValidValue(value, false, "UserName");
                ReportPropertyChanged("UserName");
                OnUserNameChanged();
            }
        }
        private global::System.String _UserName;
        partial void OnUserNameChanging(global::System.String value);
        partial void OnUserNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Avatar
        {
            get
            {
                return _Avatar;
            }
            set
            {
                OnAvatarChanging(value);
                ReportPropertyChanging("Avatar");
                _Avatar = StructuralObject.SetValidValue(value, false, "Avatar");
                ReportPropertyChanged("Avatar");
                OnAvatarChanged();
            }
        }
        private global::System.String _Avatar;
        partial void OnAvatarChanging(global::System.String value);
        partial void OnAvatarChanged();

        #endregion

        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("HabraStatsModel", "PostComments", "Post")]
        public Post Post
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Post>("HabraStatsModel.PostComments", "Post").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Post>("HabraStatsModel.PostComments", "Post").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<Post> PostReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Post>("HabraStatsModel.PostComments", "Post");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<Post>("HabraStatsModel.PostComments", "Post", value);
                }
            }
        }

        #endregion

    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="HabraStatsModel", Name="Post")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Post : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Post object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="title">Initial value of the Title property.</param>
        /// <param name="date">Initial value of the Date property.</param>
        public static Post CreatePost(global::System.Int32 id, global::System.String title, global::System.DateTime date)
        {
            Post post = new Post();
            post.Id = id;
            post.Title = title;
            post.Date = date;
            return post;
        }

        #endregion

        #region Simple Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value, "Id");
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Title
        {
            get
            {
                return _Title;
            }
            set
            {
                OnTitleChanging(value);
                ReportPropertyChanging("Title");
                _Title = StructuralObject.SetValidValue(value, false, "Title");
                ReportPropertyChanged("Title");
                OnTitleChanged();
            }
        }
        private global::System.String _Title;
        partial void OnTitleChanging(global::System.String value);
        partial void OnTitleChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Html
        {
            get
            {
                return _Html;
            }
            set
            {
                OnHtmlChanging(value);
                ReportPropertyChanging("Html");
                _Html = StructuralObject.SetValidValue(value, true, "Html");
                ReportPropertyChanged("Html");
                OnHtmlChanged();
            }
        }
        private global::System.String _Html;
        partial void OnHtmlChanging(global::System.String value);
        partial void OnHtmlChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime Date
        {
            get
            {
                return _Date;
            }
            set
            {
                OnDateChanging(value);
                ReportPropertyChanging("Date");
                _Date = StructuralObject.SetValidValue(value, "Date");
                ReportPropertyChanged("Date");
                OnDateChanged();
            }
        }
        private global::System.DateTime _Date;
        partial void OnDateChanging(global::System.DateTime value);
        partial void OnDateChanged();

        #endregion

        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("HabraStatsModel", "PostComments", "Comment")]
        public EntityCollection<Comment> Comments
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<Comment>("HabraStatsModel.PostComments", "Comment");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<Comment>("HabraStatsModel.PostComments", "Comment", value);
                }
            }
        }

        #endregion

    }

    #endregion

}