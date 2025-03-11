using Shouldly;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Xunit;

namespace MetaFac.Memory.Tests
{
    public class OctetsTests
    {
        private void AssertAreEquivalent(string chars, Octets octets)
        {
            // assert
            int length = chars.Length;
            octets.Length.ShouldBe(length);
            byte[] fromChars = Encoding.UTF8.GetBytes(chars);
            byte[] fromOctets = octets.ToArray();

            fromOctets.ShouldBeEquivalentTo(fromChars);
        }

        [Fact]
        public void NullThrows()
        {
            Assert.ThrowsAny<ArgumentNullException>(() =>
            {
                Octets? buffer = Octets.UnsafeWrap(null!);
            });
        }

        [Fact]
        public void EmptyA()
        {
            Octets buffer = Octets.Empty;
            buffer.Length.ShouldBe(0);
        }

        [Fact]
        public void EmptyB()
        {
            Octets? buffer = Octets.UnsafeWrap(ReadOnlyMemory<byte>.Empty);
            buffer.Length.ShouldBe(0);
            buffer.ShouldBeSameAs(Octets.Empty);
        }

        [Fact]
        public void EmptyC()
        {
            Octets buffer = Octets.UnsafeWrap(new byte[0])!;
            buffer.Length.ShouldBe(0);
            buffer.ShouldBeSameAs(Octets.Empty);
        }

        [Fact]
        public void ConstructFromSequence0_Empty()
        {
            ReadOnlySequence<byte> sequence = ReadOnlySequence<byte>.Empty;
            Octets buffer = new Octets(sequence);
            buffer.Length.ShouldBe(0);
            buffer.Equals(Octets.Empty).ShouldBeTrue();
        }

        [Fact]
        public void ConstructFromSequence1_SingleSegment()
        {
            ReadOnlySequence<byte> sequence = new ReadOnlySequence<byte>(new byte[] { 1, 2, 3, 4, 5 });
            sequence.IsSingleSegment.ShouldBeTrue();
            Octets buffer1 = new Octets(sequence);
            Octets buffer2 = new Octets(new byte[] { 1, 2, 3, 4, 5 });
            buffer1.Equals(buffer2).ShouldBeTrue();

            int hash1 = buffer1.GetHashCode();
            int hash2 = buffer2.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void ConstructFromSequence2_MultiSegment()
        {
            // todo
            //ReadOnlySequenceSegment<byte> segment1 = new ReadOnlySequenceSegment<byte>();
            //ReadOnlySequenceSegment<byte> segment2 = new ReadOnlySequenceSegment<byte>();
            //ReadOnlySequence<byte> sequence = new ReadOnlySequence<byte>(segment1, 0, segment2, 5);
            //sequence.IsSingleSegment.ShouldBeFalse();
            ReadOnlySequence<byte> sequence = new ReadOnlySequence<byte>(new byte[] { 1, 2, 3, 4, 5 });
            Octets buffer1 = new Octets(sequence);
            Octets buffer2 = new Octets(new byte[] { 1, 2, 3, 4, 5 });
            buffer1.Equals(buffer2).ShouldBeTrue();

            int hash1 = buffer1.GetHashCode();
            int hash2 = buffer2.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void EqualityEmpty()
        {
            Octets buffer1 = Octets.UnsafeWrap(new byte[0])!;
            Octets buffer2 = Octets.Empty;
            buffer1.ShouldBeSameAs(buffer2);
            buffer1.Equals(buffer2).ShouldBeTrue();

            int hash1 = buffer1.GetHashCode();
            int hash2 = buffer2.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void EqualityNonEmpty()
        {
            Octets buffer1 = new Octets(new byte[] { 1, 2, 3 });
            Octets buffer2 = new Octets(new byte[] { 1, 2, 3 });
            buffer1.Equals(buffer2).ShouldBeTrue();

            int hash1 = buffer1.GetHashCode();
            int hash2 = buffer2.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void Block_Create_Empty()
        {
            var s = string.Empty;
            var b = Octets.Empty;

            AssertAreEquivalent(s, b);

            b.GetHashCode().ShouldBe(0);
        }

        [Fact]
        public void Block_Create_NonEmpty()
        {
            var s = "abc";
            var b = new Octets(Encoding.UTF8.GetBytes(s));

            AssertAreEquivalent(s, b);

            b.GetHashCode().ShouldBe(192635423);
        }

        [Fact]
        public void Block_Equality_Null()
        {
            Octets? a = null;
            Octets b = Octets.Empty;

            b.Equals(a).ShouldBeFalse();
            Equals(a, b).ShouldBeFalse();
        }

        [Fact]
        public void Block_Equality_Empty()
        {
            Octets a = Octets.Empty;
            Octets b = new Octets(new byte[0]);

            a.IsEmpty.ShouldBeTrue();
            b.IsEmpty.ShouldBeTrue();
            b.GetHashCode().ShouldBe(a.GetHashCode());
            b.ShouldBeEquivalentTo(a);
            b.Equals(a).ShouldBeTrue();
            Equals(a, b).ShouldBeTrue();
        }

        [Fact]
        public void Block_Equality_SameObject()
        {
            Octets a = Octets.Empty;
            Octets b = Octets.Empty;

            b.ShouldBeSameAs(a);
            b.GetHashCode().ShouldBe(a.GetHashCode());
            b.ShouldBeEquivalentTo(a);
            b.Equals(a).ShouldBeTrue();
            Equals(a, b).ShouldBeTrue();
        }

        [Fact]
        public void Block_Equality_DifferentLength()
        {
            Octets a = new Octets(Encoding.UTF8.GetBytes("abc"));
            Octets b = new Octets(Encoding.UTF8.GetBytes("abcd"));

            b.Equals(a).ShouldBeFalse();
            Equals(a, b).ShouldBeFalse();
        }

        [Fact]
        public void Block_Equality_SameLength()
        {
            Octets a = new Octets(Encoding.UTF8.GetBytes("abc"));
            Octets b = new Octets(Encoding.UTF8.GetBytes("def"));

            b.Equals(a).ShouldBeFalse();
            Equals(a, b).ShouldBeFalse();
        }

        [Fact]
        public void Block_Equality_NonEmpty()
        {
            var s = "abc";
            var a = new Octets(Encoding.UTF8.GetBytes(s));
            var b = new Octets(Encoding.UTF8.GetBytes(s));

            b.ShouldBeEquivalentTo(a);
            b.GetHashCode().ShouldBe(a.GetHashCode());
            b.Equals(a).ShouldBeTrue();
            b.ShouldNotBeSameAs(a);
            Equals(a, b).ShouldBeTrue();
        }

        [Fact]
        public void Block_Clone_Empty()
        {
            var a = Octets.Empty;
            var b = Octets.UnsafeWrap(a.Memory);

            a.IsEmpty.ShouldBeTrue();
            b.IsEmpty.ShouldBeTrue();
            b.GetHashCode().ShouldBe(a.GetHashCode());
            b.ShouldBeEquivalentTo(a);
            b.Equals(a).ShouldBeTrue();
        }

        [Fact]
        public void Block_Enumeration()
        {
            var bytes = Encoding.UTF8.GetBytes("abc");
            var block = new Octets(bytes);

            block.Memory.Span.SequenceEqual(bytes.AsSpan()).ShouldBeTrue();

            IEnumerator<byte> e = block.GetEnumerator();
            e.Reset();
            int i = 0;
            while (e.MoveNext())
            {
                var b = e.Current;
                b.ShouldBe(bytes[i]);
                i++;
            }

            // again
            e.Reset();
            e.MoveNext().ShouldBeTrue();
            e.Current.ShouldBe(bytes[0]);
            e.MoveNext().ShouldBeTrue();
            e.Current.ShouldBe(bytes[1]);
            e.MoveNext().ShouldBeTrue();
            e.Current.ShouldBe(bytes[2]);
            e.MoveNext().ShouldBeFalse();
        }

        [Fact]
        public void Block_Clone_NonEmpty()
        {
            var a = new Octets(Encoding.UTF8.GetBytes("abc"));
            var b = Octets.UnsafeWrap(a.Memory);

            a.IsEmpty.ShouldBeFalse();
            a.Length.ShouldBe(3);
            b.IsEmpty.ShouldBeFalse();
            a.Length.ShouldBe(3);
            b.GetHashCode().ShouldBe(a.GetHashCode());
            b.ShouldBeEquivalentTo(a);
            b.Equals(a).ShouldBeTrue();
        }

        [Fact]
        public void Block_IsImmutable_FromMutableByteArray()
        {
            var readonlySource = ImmutableArray<byte>.Empty.AddRange(new byte[] { 1, 2, 3 });
            var writableSource = new byte[] { 1, 2, 3 };

            Octets block = new Octets(writableSource);
            var output1 = block.Memory.ToArray();
            output1.AsSpan().SequenceEqual(readonlySource.AsMemory().Span).ShouldBeTrue();
            output1.AsSpan().SequenceEqual(writableSource.AsSpan()).ShouldBeTrue();
            int hash1 = block.GetHashCode();

            // act
            writableSource[1] = 4;

            // assert
            var output2 = block.Memory.ToArray();
            output2.AsSpan().SequenceEqual(readonlySource.AsMemory().Span).ShouldBeTrue();
            output2.AsSpan().SequenceEqual(writableSource.AsSpan()).ShouldBeFalse();
            int hash2 = block.GetHashCode();
            hash2.ShouldBe(hash1);
        }

        [Fact]
        public void Block_UnsafeWrap_IsUnsafe()
        {
            var readonlySource = ImmutableArray<byte>.Empty.AddRange(new byte[] { 1, 2, 3 });
            var writableSource = new byte[] { 1, 2, 3 };
            var memory = new ReadOnlyMemory<byte>(writableSource);

            Octets block = Octets.UnsafeWrap(memory);
            var output1 = block.Memory.ToArray();
            output1.AsSpan().SequenceEqual(readonlySource.AsMemory().Span).ShouldBeTrue();
            output1.AsSpan().SequenceEqual(writableSource.AsSpan()).ShouldBeTrue();

            // act
            writableSource[1] = 4;

            // assert buffer has changed unsafely
            var output2 = block.Memory.ToArray();
            output2.AsSpan().SequenceEqual(readonlySource.AsMemory().Span).ShouldBeFalse();
            output2.AsSpan().SequenceEqual(writableSource.AsSpan()).ShouldBeTrue();
        }

        [Fact]
        public void Block_UnsafeWrap_CanBeMadeSafe()
        {
            var mutable = new byte[] { 1, 2, 3 };
            var memory = new ReadOnlyMemory<byte>(mutable);

            // arrange
            Octets unsafeOrig = Octets.UnsafeWrap(memory);
            Octets unsafeCopy = Octets.UnsafeWrap(unsafeOrig.Memory);
            Octets safeCopy = new Octets(unsafeOrig.Memory.Span);

            // act
            mutable[1] = 4;

            // assert
            unsafeCopy.Equals(unsafeOrig).ShouldBeTrue();
            unsafeCopy.GetHashCode().ShouldBe(unsafeOrig.GetHashCode());

            safeCopy.Equals(unsafeOrig).ShouldBeFalse();
            safeCopy.GetHashCode().ShouldNotBe(unsafeOrig.GetHashCode());
        }

        [Fact]
        public void GetHeadEmpty()
        {
            Octets orig = Octets.Empty;
            (var head, var body) = orig.GetHead(0);
            head.Equals(Octets.Empty).ShouldBeTrue();
            body.Equals(Octets.Empty).ShouldBeTrue();
            var copy = new Octets(head.Memory.Span, body.Memory.Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadNonEmptyA()
        {
            Octets orig = new Octets(new byte[] { 1, 2, 3 });
            (var head, var body) = orig.GetHead(1);
            head.Length.ShouldBe(1);
            var headBytes = head.Memory.ToArray();
            ((int)headBytes[0]).ShouldBe(1);
            body.Length.ShouldBe(2);
            var bodyBytes = body.Memory.ToArray();
            ((int)bodyBytes[0]).ShouldBe(2);
            ((int)bodyBytes[1]).ShouldBe(3);
            var copy = new Octets(head.Memory.Span, body.Memory.Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadNonEmptyB()
        {
            Octets orig = new Octets(new byte[] { 1, 2 }, new byte[] { 3, 4 });
            (var head, var body) = orig.GetHead(1);
            // head
            head.Length.ShouldBe(1);
            var headBytes = head.Memory.ToArray();
            ((int)headBytes[0]).ShouldBe(1);
            // body
            body.Length.ShouldBe(3);
            var bodyBytes = body.Memory.ToArray();
            ((int)bodyBytes[0]).ShouldBe(2);
            ((int)bodyBytes[1]).ShouldBe(3);
            ((int)bodyBytes[2]).ShouldBe(4);

            var copy = new Octets(head.Memory.Span, body.Memory.Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadTailEmpty()
        {
            Octets orig = Octets.Empty;
            (var head, var body, var tail) = orig.GetHeadTail(0, 0);
            head.Equals(Octets.Empty).ShouldBeTrue();
            body.Equals(Octets.Empty).ShouldBeTrue();
            tail.Equals(Octets.Empty).ShouldBeTrue();
            var copy = new Octets(head.Memory.Span, body.Memory.Span, tail.Memory.Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadTailNonEmptyA()
        {
            Octets orig = new Octets(new byte[] { 1, 2, 3 });
            (var head, var body, var tail) = orig.GetHeadTail(1, 1);
            head.Length.ShouldBe(1);
            var headBytes = head.Memory.ToArray();
            ((int)headBytes[0]).ShouldBe(1);
            body.Length.ShouldBe(1);
            var bodyBytes = body.Memory.ToArray();
            ((int)bodyBytes[0]).ShouldBe(2);
            tail.Length.ShouldBe(1);
            var tailBytes = tail.Memory.ToArray();
            ((int)tailBytes[0]).ShouldBe(3);
            var copy = new Octets(head.Memory.Span, body.Memory.Span, tail.Memory.Span);
            copy.Equals(orig).ShouldBeTrue();
        }

        [Fact]
        public void GetHeadTailNonEmptyB()
        {
            Octets orig = new Octets(new byte[] { 1, 2, 3, 4 }, new byte[] { 5, 6, 7, 8 });
            (var head, var body, var tail) = orig.GetHeadTail(2, 4);
            // head
            head.Length.ShouldBe(2);
            var headBytes = head.Memory.ToArray();
            ((int)headBytes[0]).ShouldBe(1);
            ((int)headBytes[1]).ShouldBe(2);
            // body
            body.Length.ShouldBe(4);
            var bodyBytes = body.Memory.ToArray();
            ((int)bodyBytes[0]).ShouldBe(3);
            ((int)bodyBytes[1]).ShouldBe(4);
            ((int)bodyBytes[2]).ShouldBe(5);
            ((int)bodyBytes[3]).ShouldBe(6);
            // tail
            tail.Length.ShouldBe(2);
            var tailBytes = tail.Memory.ToArray();
            ((int)tailBytes[0]).ShouldBe(7);
            ((int)tailBytes[1]).ShouldBe(8);

            var copy = new Octets(head.Memory.Span, body.Memory.Span, tail.Memory.Span);
            copy.Equals(orig).ShouldBeTrue();
        }
    }
}