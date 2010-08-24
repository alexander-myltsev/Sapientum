namespace Sapientum

open System
open System.IO
open System.Text
open System.Text.RegularExpressions
open System.Security.Cryptography

module Helper =
  
    type StringBuilder with
        static member toString (sb:StringBuilder) = sb.ToString()
        static member append (str:String) (sb:StringBuilder) = sb.Append str

    type String with
        member this.ToMD5Hash() = 
            let md5Provider = new MD5CryptoServiceProvider()
            let hash = this |> Encoding.UTF8.GetBytes |> md5Provider.ComputeHash
            Array.fold 
                (fun (sb:StringBuilder) (b:byte) -> b.ToString("x2") |> sb.Append) 
                (new StringBuilder()) 
                hash
            |> StringBuilder.toString

    let (=~)  s (re:Regex) = re.IsMatch(s)
    let (<>~) s (re:Regex) = not (s =~ re)
    let (=~.) s (re:Regex) = re.Split(s)
    let (=~!) s (re:Regex) = re.Match(s)
  
    let regex s = new Regex(s,RegexOptions.IgnoreCase ||| RegexOptions.Singleline)  
    let (|ParseRegex|_|) rg str =
        let m = str =~! regex rg
        if m.Success then Some (List.tail [ for x in m.Groups -> x.Value ]) else None
  
    let streamReader (e:Encoding) (s:Stream) = new StreamReader(s,e)
    type StreamReader with 
        static member readToEnd (sr:StreamReader) = sr.ReadToEnd()

    let inline Parse< ^T when ^T : (static member Parse : string -> ^T) > (stringToParse:string) : ^T option = 
//        let mutable res = LanguagePrimitives.GenericZero
//        let v = ( ^T : (static member TryParse : string*byref< ^T> -> ^T) (stringToParse,&res))
//        Some v
        try
            let v = ( ^T : (static member Parse : string -> ^T) stringToParse)
            Some v
        with
        | :? Exception as ex -> None