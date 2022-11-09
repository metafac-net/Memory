﻿using FluentAssertions;
using System;
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
            octets.Length.Should().Be(length);
            byte[] fromChars = Encoding.UTF8.GetBytes(chars);
            byte[] fromOctets = octets.ToArray();

            fromOctets.Should().BeEquivalentTo(fromChars);
        }

        [Fact]
        public void Null()
        {
            byte[]? bytes = null;
            Octets? buffer = Octets.UnsafeWrap(bytes);
            buffer.Should().BeNull();
        }

        [Fact]
        public void EmptyA()
        {
            Octets buffer = Octets.Empty;
            buffer.Length.Should().Be(0);
        }

        [Fact]
        public void EmptyB()
        {
            Octets? buffer = Octets.UnsafeWrap(ReadOnlyMemory<byte>.Empty);
            buffer.Length.Should().Be(0);
            buffer.Should().BeSameAs(Octets.Empty);
        }

        [Fact]
        public void EmptyC()
        {
            Octets buffer = Octets.UnsafeWrap(new byte[0])!;
            buffer.Length.Should().Be(0);
            buffer.Should().BeSameAs(Octets.Empty);
        }

        [Fact]
        public void EqualityEmpty()
        {
            Octets buffer1 = Octets.UnsafeWrap(new byte[0])!;
            Octets buffer2 = Octets.Empty;
            buffer1.Should().BeSameAs(buffer2);
            buffer1.Equals(buffer2).Should().BeTrue();

            int hash1 = buffer1.GetHashCode();
            int hash2 = buffer2.GetHashCode();
            hash2.Should().Be(hash1);
        }

        [Fact]
        public void EqualityNonEmpty()
        {
            Octets buffer1 = new Octets(new byte[] { 1, 2, 3 });
            Octets buffer2 = new Octets(new byte[] { 1, 2, 3 });
            buffer1.Equals(buffer2).Should().BeTrue();

            int hash1 = buffer1.GetHashCode();
            int hash2 = buffer2.GetHashCode();
            hash2.Should().Be(hash1);
        }

        [Fact]
        public void Block_Create_Empty()
        {
            var s = string.Empty;
            var b = Octets.Empty;

            AssertAreEquivalent(s, b);

            b.GetHashCode().Should().Be(0);
        }

        [Fact]
        public void Block_Create_NonEmpty()
        {
            var s = "abc";
            var b = new Octets(Encoding.UTF8.GetBytes(s));

            AssertAreEquivalent(s, b);

            b.GetHashCode().Should().Be(192635423);
        }

        [Fact]
        public void Block_Equality_Null()
        {
            Octets? a = null;
            Octets b = Octets.Empty;

            b.Equals(a).Should().BeFalse();
            Equals(a, b).Should().BeFalse();
        }

        [Fact]
        public void Block_Equality_Empty()
        {
            Octets a = Octets.Empty;
            Octets b = new Octets(new byte[0]);

            a.IsEmpty.Should().BeTrue();
            b.IsEmpty.Should().BeTrue();
            b.GetHashCode().Should().Be(a.GetHashCode());
            b.Should().BeEquivalentTo(a);
            b.Equals(a).Should().BeTrue();
            Equals(a, b).Should().BeTrue();
        }

        [Fact]
        public void Block_Equality_SameObject()
        {
            Octets a = Octets.Empty;
            Octets b = Octets.Empty;

            b.Should().BeSameAs(a);
            b.GetHashCode().Should().Be(a.GetHashCode());
            b.Should().BeEquivalentTo(a);
            b.Equals(a).Should().BeTrue();
            Equals(a, b).Should().BeTrue();
        }

        [Fact]
        public void Block_Equality_DifferentLength()
        {
            Octets a = new Octets(Encoding.UTF8.GetBytes("abc"));
            Octets b = new Octets(Encoding.UTF8.GetBytes("abcd"));

            b.Equals(a).Should().BeFalse();
            Equals(a, b).Should().BeFalse();
        }

        [Fact]
        public void Block_Equality_SameLength()
        {
            Octets a = new Octets(Encoding.UTF8.GetBytes("abc"));
            Octets b = new Octets(Encoding.UTF8.GetBytes("def"));

            b.Equals(a).Should().BeFalse();
            Equals(a, b).Should().BeFalse();
        }

        [Fact]
        public void Block_Equality_NonEmpty()
        {
            var s = "abc";
            var a = new Octets(Encoding.UTF8.GetBytes(s));
            var b = new Octets(Encoding.UTF8.GetBytes(s));

            b.Should().BeEquivalentTo(a);
            b.GetHashCode().Should().Be(a.GetHashCode());
            b.Equals(a).Should().BeTrue();
            b.Should().NotBeSameAs(a);
            Equals(a, b).Should().BeTrue();
        }

        [Fact]
        public void Block_Clone_Empty()
        {
            var a = Octets.Empty;
            var b = Octets.UnsafeWrap(a.Memory);

            a.IsEmpty.Should().BeTrue();
            b.IsEmpty.Should().BeTrue();
            b.GetHashCode().Should().Be(a.GetHashCode());
            b.Should().BeEquivalentTo(a);
            b.Equals(a).Should().BeTrue();
        }

        [Fact]
        public void Block_Enumeration()
        {
            var bytes = Encoding.UTF8.GetBytes("abc");
            var block = new Octets(bytes);

            block.Should().BeEquivalentTo(bytes);
            block.Should().BeEquivalentTo(bytes);

            IEnumerator<byte> e = block.GetEnumerator();
            e.Reset();
            int i = 0;
            while (e.MoveNext())
            {
                var b = e.Current;
                b.Should().Be(bytes[i]);
                i++;
            }

            // again
            e.Reset();
            e.MoveNext().Should().BeTrue();
            e.Current.Should().Be(bytes[0]);
            e.MoveNext().Should().BeTrue();
            e.Current.Should().Be(bytes[1]);
            e.MoveNext().Should().BeTrue();
            e.Current.Should().Be(bytes[2]);
            e.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void Block_Clone_NonEmpty()
        {
            var a = new Octets(Encoding.UTF8.GetBytes("abc"));
            var b = Octets.UnsafeWrap(a.Memory);

            a.IsEmpty.Should().BeFalse();
            a.Length.Should().Be(3);
            b.IsEmpty.Should().BeFalse();
            a.Length.Should().Be(3);
            b.GetHashCode().Should().Be(a.GetHashCode());
            b.Should().BeEquivalentTo(a);
            b.Equals(a).Should().BeTrue();
        }

        [Fact]
        public void Block_IsImmutable_FromByteArray()
        {
            var immutable = ImmutableArray<byte>.Empty.AddRange(new byte[] { 1, 2, 3 });
            var mutable = new byte[] { 1, 2, 3 };

            Octets block = new Octets(mutable);
            var output1 = block.Memory.ToArray();
            output1.Should().BeEquivalentTo(immutable);
            output1.Should().BeEquivalentTo(mutable);
            int hash1 = block.GetHashCode();

            // act
            mutable[1] = 4;

            // assert
            var output2 = block.Memory.ToArray();
            output2.Should().BeEquivalentTo(immutable);
            output2.Should().NotBeEquivalentTo(mutable);
            int hash2 = block.GetHashCode();
            hash2.Should().Be(hash1);
        }

        [Fact]
        public void Block_IsImmutable_FromArraySegment()
        {
            var immutable = ImmutableArray<byte>.Empty.AddRange(new byte[] { 1, 2, 3 });
            var mutable = new byte[] { 1, 2, 3 };
            var segment = new ArraySegment<byte>(mutable);

            Octets block = new Octets(segment);
            var output1 = block.Memory.ToArray();
            output1.Should().BeEquivalentTo(immutable);
            output1.Should().BeEquivalentTo(mutable);
            int hash1 = block.GetHashCode();

            // act
            mutable[1] = 4;

            // assert
            var output2 = block.Memory.ToArray();
            output2.Should().BeEquivalentTo(immutable);
            output2.Should().NotBeEquivalentTo(mutable);
            int hash2 = block.GetHashCode();
            hash2.Should().Be(hash1);
        }

        [Fact]
        public void Block_IsImmutable_FromSpan()
        {
            var immutable = ImmutableArray<byte>.Empty.AddRange(new byte[] { 1, 2, 3 });
            var mutable = new byte[] { 1, 2, 3 };
            var span = new ReadOnlySpan<byte>(mutable);

            Octets block = new Octets(span);
            var output1 = block.Memory.ToArray();
            output1.Should().BeEquivalentTo(immutable);
            output1.Should().BeEquivalentTo(mutable);
            int hash1 = block.GetHashCode();

            // act
            mutable[1] = 4;

            // assert
            var output2 = block.Memory.ToArray();
            output2.Should().BeEquivalentTo(immutable);
            output2.Should().NotBeEquivalentTo(mutable);
            int hash2 = block.GetHashCode();
            hash2.Should().Be(hash1);
        }

        [Fact]
        public void Block_UnsafeWrap_IsNotImmutable()
        {
            var immutable = ImmutableArray<byte>.Empty.AddRange(new byte[] { 1, 2, 3 });
            var mutable = new byte[] { 1, 2, 3 };
            var memory = new ReadOnlyMemory<byte>(mutable);

            Octets block = Octets.UnsafeWrap(memory);
            var output1 = block.Memory.ToArray();
            output1.Should().BeEquivalentTo(immutable);
            output1.Should().BeEquivalentTo(mutable);

            // act
            mutable[1] = 4;

            // assert
            var output2 = block.Memory.ToArray();
            output2.Should().NotBeEquivalentTo(immutable);
            output2.Should().BeEquivalentTo(mutable);
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
            unsafeCopy.Equals(unsafeOrig).Should().BeTrue();
            unsafeCopy.GetHashCode().Should().Be(unsafeOrig.GetHashCode());

            safeCopy.Equals(unsafeOrig).Should().BeFalse();
            safeCopy.GetHashCode().Should().NotBe(unsafeOrig.GetHashCode());
        }

        [Fact]
        public void GetHeadEmpty()
        {
            Octets orig = Octets.Empty;
            (var head, var body) = orig.GetHead(0);
            head.Equals(Octets.Empty).Should().BeTrue();
            body.Equals(Octets.Empty).Should().BeTrue();
            var copy = new Octets(head.Memory.Span, body.Memory.Span);
            copy.Equals(orig).Should().BeTrue();
        }

        [Fact]
        public void GetHeadNonEmptyA()
        {
            Octets orig = new Octets(new byte[] { 1, 2, 3 });
            (var head, var body) = orig.GetHead(1);
            head.Length.Should().Be(1);
            var headBytes = head.Memory.ToArray();
            headBytes[0].Should().Be(1);
            body.Length.Should().Be(2);
            var bodyBytes = body.Memory.ToArray();
            bodyBytes[0].Should().Be(2);
            bodyBytes[1].Should().Be(3);
            var copy = new Octets(head.Memory.Span, body.Memory.Span);
            copy.Equals(orig).Should().BeTrue();
        }

        [Fact]
        public void GetHeadNonEmptyB()
        {
            Octets orig = new Octets(new byte[] { 1, 2 }, new byte[] { 3, 4 });
            (var head, var body) = orig.GetHead(1);
            // head
            head.Length.Should().Be(1);
            var headBytes = head.Memory.ToArray();
            headBytes[0].Should().Be(1);
            // body
            body.Length.Should().Be(3);
            var bodyBytes = body.Memory.ToArray();
            bodyBytes[0].Should().Be(2);
            bodyBytes[1].Should().Be(3);
            bodyBytes[2].Should().Be(4);

            var copy = new Octets(head.Memory.Span, body.Memory.Span);
            copy.Equals(orig).Should().BeTrue();
        }

        [Fact]
        public void GetHeadTailEmpty()
        {
            Octets orig = Octets.Empty;
            (var head, var body, var tail) = orig.GetHeadTail(0, 0);
            head.Equals(Octets.Empty).Should().BeTrue();
            body.Equals(Octets.Empty).Should().BeTrue();
            tail.Equals(Octets.Empty).Should().BeTrue();
            var copy = new Octets(head.Memory.Span, body.Memory.Span, tail.Memory.Span);
            copy.Equals(orig).Should().BeTrue();
        }

        [Fact]
        public void GetHeadTailNonEmptyA()
        {
            Octets orig = new Octets(new byte[] { 1, 2, 3 });
            (var head, var body, var tail) = orig.GetHeadTail(1, 1);
            head.Length.Should().Be(1);
            var headBytes = head.Memory.ToArray();
            headBytes[0].Should().Be(1);
            body.Length.Should().Be(1);
            var bodyBytes = body.Memory.ToArray();
            bodyBytes[0].Should().Be(2);
            tail.Length.Should().Be(1);
            var tailBytes = tail.Memory.ToArray();
            tailBytes[0].Should().Be(3);
            var copy = new Octets(head.Memory.Span, body.Memory.Span, tail.Memory.Span);
            copy.Equals(orig).Should().BeTrue();
        }

        [Fact]
        public void GetHeadTailNonEmptyB()
        {
            Octets orig = new Octets(new byte[] { 1, 2, 3, 4 }, new byte[] { 5, 6, 7, 8 });
            (var head, var body, var tail) = orig.GetHeadTail(2, 4);
            // head
            head.Length.Should().Be(2);
            var headBytes = head.Memory.ToArray();
            headBytes[0].Should().Be(1);
            headBytes[1].Should().Be(2);
            // body
            body.Length.Should().Be(4);
            var bodyBytes = body.Memory.ToArray();
            bodyBytes[0].Should().Be(3);
            bodyBytes[1].Should().Be(4);
            bodyBytes[2].Should().Be(5);
            bodyBytes[3].Should().Be(6);
            // tail
            tail.Length.Should().Be(2);
            var tailBytes = tail.Memory.ToArray();
            tailBytes[0].Should().Be(7);
            tailBytes[1].Should().Be(8);

            var copy = new Octets(head.Memory.Span, body.Memory.Span, tail.Memory.Span);
            copy.Equals(orig).Should().BeTrue();
        }
    }
}