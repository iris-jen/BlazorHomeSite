using BlazorHomeSite.Data.Domain;
using FluentAssertions;

namespace Tests.Data;

public class CommentModelTests
{
    [Fact]
    public void GivenComment_WhenContentUpdated_ThenSet()
    {
        var comment = new Comment("abc", 99);
        comment.Content.Should().Be("abc");
        comment.UpdateContent("cba");
        comment.Content.Should().Be("cba");
    }

    [Fact]
    public void GivenComment_WithContentOver512Chars_ThenThrows()
        => Assert.Throws<ArgumentException>(() =>
            {
                var badCommentLength = @"66862587697863341479588268757916455365755136431823145628829
                                             48377818289816245824379448718376568721283899254831734676996
                                             94869743365246515861946776669348353732889282893515289871139
                                             35438289224728237189822941685836637259873943376985313311839
                                             56951317917882899896717238237269453214332218872154225827395
                                             37856518975855647992361933442363277657537934967795671737697";

                badCommentLength.Length.Should().BeGreaterThan(512);
                _ = new Comment(badCommentLength, 1);
            });

    [Fact]
    public void GivenComment_WithCorrectConstruction_ThenPropertiesSet()
    {
        var contnet = @"woweeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
                        😀 😃 😄 😁 😆 😅
                        😂 🤣 😊 😇 🙂 🙃 😉 😌
                        😍 🥰 😘 😗 😙 😚 😋 😛 😝
                        😜";
        var comment = new Comment(contnet, 1);
        comment.Content.Should().Be(contnet);
        comment.UserId.Should().Be(1);
    }

    [Fact]
    public void GivenComment_WithEmptyContent_ThenThrows()
        => Assert.Throws<ArgumentException>(() =>
            {
                _ = new Comment(string.Empty, 1);
            });
}