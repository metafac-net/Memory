# Memory

[![Build-Deploy](https://github.com/metafac-net/Memory/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/metafac-net/Memory/actions/workflows/dotnet.yml)

*Note: To be deprecated soon. Types will move to DataFac.Memory.*

Memory efficient types:
- Octets: An immutable reference type that wraps a ReadOnlyMemory<byte> buffer.
- Blocks: Structs that divide memory into a binary tree. Sizes from 1B to 8KB.