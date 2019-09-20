# OscCore

[![CircleCI](https://circleci.com/gh/tilde-love/osc-core.svg?style=svg)](https://circleci.com/gh/tilde-love/osc-core) 

**A performant Open Sound Control library for .NET Standard from the creator of Rug.Osc**

## tl;dr 

**If you want ease of use and fast results use Rug.Osc. If you can do the work and want fast code use OscCore.**

The motivation for this new library is: 

* Allow fast reading and writing of OSC messages and bundles without boxing of types or excessive memory allocation.
* Decouple message format from transport layer. There in no UDP / TCP / Serial code in this library.
* To use the lowest possible version of .NET Standard (currently 1.3) to allow maximum portability.  
* To move away from the monolith that was Rug.Osc. 

Differences over Rug.Osc: 

* Thread safety, Rug.Osc was aggressively thread safe. OscCore is **NOT thread safe!** you will have to deal with that stuff on your own.
* Ease of use, Rug.Osc was very friendly. OscCore is **NOT your friend** it will allow you to create invalid messages and parse junk if you are not careful.
* Transport layer, Rug.Osc had all the transport layer stuff built in, it is a one stop shop for sending and receiving OSC messages. **OscCore does non of this**, it is expected that you will use some other library to get data on and off the wire.
* Osc Address routing, Rug.Osc has mechanisms for routing incoming messages to delegates. **OscCore does not provide routing**, you will have to devise the best method for how this happens in the context of your application.   

## Why would I use this belligerent library?

OscCore is:
 
* Performant, using the 'Raw' api is typically more that 2x faster than Rug.Osc for read operations.  
* Uses minimal memory footprint / copying for normal operation. 
