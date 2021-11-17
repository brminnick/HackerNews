[![MAUI](https://github.com/brminnick/HackerNews/actions/workflows/maui.yml/badge.svg)](https://github.com/brminnick/HackerNews/actions/workflows/maui.yml)

# HackerNews

A .NET MAUI app for displaying the top posts on Hacker News.

This app demonstrates how to use `IAsyncEnumerable` + C# 8.0 to improve performance. Thanks to `IAsyncEnumerable`, the items are added to the list as soon as they're available making the app feel faster and more responsive.

This app also uses the [Text Analytics API](https://azure.microsoft.com/services/cognitive-services/text-analytics?WT.mc_id=mobile-0000-bramin) from [Microsoft Cognitive Services](https://azure.microsoft.com/services/cognitive-services?WT.mc_id=mobile-0000-bramin) to analyze the sentiment of each headline. 
- 😃 Headline is Happy 
- ☹️ Headline is Sad 
- 😐 Headline is Neither Happy or Sad

![Hacker News Demo](https://user-images.githubusercontent.com/13558917/66956918-2873bb80-f01a-11e9-839c-6e935c0b606c.gif)
