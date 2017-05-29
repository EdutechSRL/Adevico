Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.Business
Imports lm.Comol.Core.DomainModel.Helpers

Namespace Business
    Public Class BaseHelperExport

#Region "Translations"
        Private m_CommonTranslations As New Dictionary(Of QuestionnaireExportTranslations, String)
        Private Property CommonTranslations() As Dictionary(Of QuestionnaireExportTranslations, String)
            Get
                Return m_CommonTranslations
            End Get
            Set(value As Dictionary(Of QuestionnaireExportTranslations, String))
                m_CommonTranslations = value
            End Set
        End Property

        Private ReadOnly Property LibraryIdentifier
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.LibraryIdentifier)
            End Get
        End Property
        Private ReadOnly Property PageIdentifier
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.PageIdentifier)
            End Get
        End Property
        Private ReadOnly Property QuestionIdentifier
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.QuestionIdentifier)
            End Get
        End Property
        Private ReadOnly Property DeletedQuestionIdentifier
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.DeletedQuestionIdentifier)
            End Get
        End Property
        Private ReadOnly Property OptionIdentifier
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.OptionIdentifier)
            End Get
        End Property
        Private ReadOnly Property FreeTextIdentifier
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.FreeTextIdentifier)
            End Get
        End Property
        Private ReadOnly Property ItemEmpty
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.ItemEmpty)
            End Get
        End Property
        Private ReadOnly Property NullItem
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.NullItem)
            End Get
        End Property
        Private ReadOnly Property QuestionNotUsed
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.QuestionNotUsed)
            End Get
        End Property
        Private ReadOnly Property QuestionNotUsedIdentifier
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.QuestionNotUsedIdentifier)
            End Get
        End Property

        Private ReadOnly Property CellTitleIdQuestion
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleIdQuestion)
            End Get
        End Property
        Private ReadOnly Property CellTitleIdOption
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleIdOption)
            End Get
        End Property
        Private ReadOnly Property CellTitleOptionNumber
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleOptionNumber)
            End Get
        End Property
        Private ReadOnly Property CellTitleOptionValue
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleOptionValue)
            End Get
        End Property
        Private ReadOnly Property CellTitleOptionFreeText
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleOptionFreeText)
            End Get
        End Property
        Private ReadOnly Property CellTitleEvaluation
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleEvaluation)
            End Get
        End Property
        Private ReadOnly Property CellTitleOptionCorrect
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleOptionCorrect)
            End Get
        End Property
        Private ReadOnly Property CellTitleOptionPoints
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleOptionPoints)
            End Get
        End Property

        Private ReadOnly Property CellTitleIdUser
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleIdUser)
            End Get
        End Property
        Private ReadOnly Property CellTitleIdInvitedUser
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleIdInvitedUser)
            End Get
        End Property

        Private ReadOnly Property CellTitleStartedOn
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleStartedOn)
            End Get
        End Property
        Private ReadOnly Property CellTitleModifiedOn
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleModifiedOn)
            End Get
        End Property
        Private ReadOnly Property CellTitleEndedOn
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleEndedOn)
            End Get
        End Property
        Private ReadOnly Property CellTitleOptionText
            Get
                Return CommonTranslations(QuestionnaireExportTranslations.CellTitleOptionText)
            End Get
        End Property

#End Region

        Public Sub New()
        End Sub
        Public Sub New(translations As Dictionary(Of QuestionnaireExportTranslations, String))
            CommonTranslations = translations
        End Sub

#Region "Export Questionnaire"
        Public Sub QuestionsRender(ByVal userRequiringExport As Person, ByVal dto As dtoExportQuestionnaire, libraryNumber As Integer, ByVal questions As List(Of Domanda), answers As Dictionary(Of Long, QuestionnaireAnswer))
            For Each oDom As Domanda In questions
                Select Case oDom.tipo
                    Case Domanda.TipoDomanda.DropDown
                        ConvertDropDownToString(oDom, dto, libraryNumber, answers)
                    Case Domanda.TipoDomanda.Multipla
                        ConvertMultiplaToString(oDom, dto, libraryNumber, answers)
                    Case Domanda.TipoDomanda.Numerica
                        ConvertNumericaToString(oDom, dto, libraryNumber, answers)
                    Case Domanda.TipoDomanda.Rating
                        ConvertRatingToString(oDom, dto, libraryNumber, answers)
                    Case Domanda.TipoDomanda.TestoLibero
                        ConvertTestoLiberoToString(oDom, dto, libraryNumber, answers)
                End Select
            Next
        End Sub

        Public Sub QuestionsRender(ByVal currentuser As Person, ByVal displayTaxCode As Boolean, ByVal allowFullExport As Boolean, ByVal dto As dtoExportQuestionnaire, ByVal displayNames As Dictionary(Of String, dtoDisplayName), ByVal anonymousUser As String, qAnswers As List(Of dtoFullUserAnswerItem), answers As Dictionary(Of Long, QuestionnaireAnswer))
            dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleIdAnswer))

            'Dim addTaxCode As Boolean = False
            If allowFullExport Then
                dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleSurname))
                dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleName))
                If displayTaxCode Then
                    Select Case currentuser.TypeID
                        Case CInt(UserTypeStandard.SysAdmin), CInt(UserTypeStandard.Administrator), CInt(UserTypeStandard.Administrative)
                            dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleTaxCode))
                        Case Else
                            displayTaxCode = False
                    End Select
                End If
                dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleOtherInfos))
            Else
                dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleUserName))
            End If


            dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleIdUser))
            dto.Cells.Add(CommonTranslations(QuestionnaireExportTranslations.CellTitleIdInvitedUser))
            dto.Cells.Add(CellTitleStartedOn)
            dto.Cells.Add(CellTitleModifiedOn)
            dto.Cells.Add(CellTitleEndedOn)

            dto.Cells.Add(CellTitleIdQuestion)
            '  dto.Cells.Add(CellTitleOptionCorrect)
            dto.Cells.Add(CellTitleIdOption)
            dto.Cells.Add(CellTitleOptionNumber)
            dto.Cells.Add(CellTitleOptionValue)
            dto.Cells.Add(CellTitleOptionText)

            dto.Cells.Add(CellTitleOptionFreeText)
            dto.Cells.Add(CellTitleEvaluation)

            '  dto.Cells.Add(CellTitleOptionPoints)

            Dim index As Long = 1
            For Each qAnswerItem As dtoFullUserAnswerItem In qAnswers
                Dim answer As QuestionnaireAnswer = answers(qAnswerItem.Id)

                If Not IsNothing(answer) Then
                    Dim info As New List(Of String)
                    info.Add(answer.IdUser)
                    info.Add(answer.IdInvitedUser)
                    If qAnswerItem.Answer.StartedOn.HasValue Then
                        info.Add(qAnswerItem.Answer.StartedOn.Value)
                    Else
                        info.Add("")
                    End If
                    If qAnswerItem.Answer.ModifiedOn.HasValue Then
                        info.Add(qAnswerItem.Answer.ModifiedOn.Value)
                    Else
                        info.Add("")
                    End If
                    If qAnswerItem.Answer.CompletedOn.HasValue Then
                        info.Add(qAnswerItem.Answer.CompletedOn.Value)
                    Else
                        info.Add("")
                    End If

                    For Each item As LazyAssociatedQuestion In answer.Questions.OrderBy(Function(q) q.Number).ThenBy(Function(q) q.IdQuestion)
                        Dim row As dtoExportRow = GenerateAnswerRow(index, qAnswerItem, displayNames, anonymousUser)
                        row.Items.AddRange(info)
                        row.Items.Add(item.IdQuestion)
                        Dim idQuestion As Integer = item.IdQuestion
                        Dim questionAnswer As QuestionAnswer = answer.Answers.Where(Function(a) a.IdQuestion.Equals(idQuestion)).FirstOrDefault()

                        If IsNothing(questionAnswer) Then
                            For i As Integer = 1 To 6
                                row.Items.Add("")
                            Next
                        Else
                            row.Items.Add(questionAnswer.IdQuestionOption)

                            Select Case questionAnswer.QuestionType
                                Case Domanda.TipoDomanda.DropDown
                                    '  row.Items.Add("")
                                    row.Items.Add(questionAnswer.OptionNumber)
                                    row.Items.Add(questionAnswer.OptionText)
                                    row.Items.Add("")
                                    row.Items.Add("")

                                    row.Items.Add("")

                                    row.Items.Add("")
                                    'dto.Cells.Add(CellTitleOptionCorrect)
                                    'dto.Cells.Add(CellTitleOptionPoints)

                                Case Domanda.TipoDomanda.Multipla
                                    '  row.Items.Add("")
                                    row.Items.Add(questionAnswer.OptionNumber)
                                    row.Items.Add("")
                                    row.Items.Add("")

                                    row.Items.Add(questionAnswer.OptionText)
                                    row.Items.Add("")
                                    row.Items.Add("")
                                    'dto.Cells.Add(CellTitleOptionCorrect)
                                    'dto.Cells.Add(CellTitleOptionPoints)
                                Case Domanda.TipoDomanda.Numerica
                                    '  row.Items.Add("")
                                    row.Items.Add(questionAnswer.OptionNumber)
                                    row.Items.Add(questionAnswer.Value)
                                    row.Items.Add(questionAnswer.OptionNumber)

                                    row.Items.Add("")
                                    row.Items.Add("")
                                    row.Items.Add("")
                                    'dto.Cells.Add(CellTitleOptionCorrect)
                                    'dto.Cells.Add(CellTitleOptionPoints)

                                Case Domanda.TipoDomanda.Rating
                                    '  row.Items.Add("")
                                    row.Items.Add(questionAnswer.OptionNumber)
                                    row.Items.Add(questionAnswer.Value)
                                    row.Items.Add("")

                                    row.Items.Add(questionAnswer.OptionText)
                                    row.Items.Add(questionAnswer.Evaluation)
                                    row.Items.Add("")

                                    'dto.Cells.Add(CellTitleOptionCorrect)
                                    'dto.Cells.Add(CellTitleOptionPoints)

                                Case Domanda.TipoDomanda.TestoLibero
                                    '  row.Items.Add("")
                                    row.Items.Add(questionAnswer.OptionNumber)
                                    row.Items.Add(questionAnswer.Value)
                                    row.Items.Add("")

                                    row.Items.Add(questionAnswer.OptionText)
                                    row.Items.Add(questionAnswer.Evaluation)

                                    row.Items.Add("")
                                    'dto.Cells.Add(CellTitleOptionCorrect)
                                    'dto.Cells.Add(CellTitleOptionPoints)
                            End Select
                        End If
                        index += 1
                        dto.Rows.Add(row)
                    Next
                Else
                    Dim row As dtoExportRow = GenerateAnswerRow(index, qAnswerItem, displayNames, anonymousUser)
                    For i As Integer = 1 To 13
                        row.Items.Add("")
                    Next
                    index += 1
                    dto.Rows.Add(row)
                End If
            Next
        End Sub
        Private Function GenerateAnswerRow(index As Integer, qAnswerItem As dtoFullUserAnswerItem, ByVal displayNames As Dictionary(Of String, dtoDisplayName), ByVal anonymousUser As String) As dtoExportRow
            Dim row As dtoExportRow
            If Not displayNames.Values.Any() Then
                row = New dtoExportRow() With {.Index = index, .IdAnswer = qAnswerItem.Id, .DisplayInfo = New dtoDisplayName(anonymousUser)}
            Else
                If qAnswerItem.Answer.IdInvitedUser.HasValue AndAlso displayNames.ContainsKey("i_" & qAnswerItem.Answer.IdInvitedUser) Then
                    row = New dtoExportRow() With {.Index = index, .IdAnswer = qAnswerItem.Id, .DisplayInfo = displayNames("i_" & qAnswerItem.Answer.IdInvitedUser)}
                ElseIf qAnswerItem.Answer.IdPerson > 0 AndAlso displayNames.ContainsKey("p_" & qAnswerItem.Answer.IdPerson) Then
                    row = New dtoExportRow() With {.Index = index, .IdAnswer = qAnswerItem.Id, .DisplayInfo = displayNames("p_" & qAnswerItem.Answer.IdPerson)}
                ElseIf displayNames.ContainsKey("p_0") Then
                    row = New dtoExportRow() With {.Index = index, .IdAnswer = qAnswerItem.Id, .DisplayInfo = displayNames("p_0")}
                Else
                    row = New dtoExportRow() With {.Index = index, .IdAnswer = qAnswerItem.Id, .DisplayInfo = New dtoDisplayName(anonymousUser)}
                End If
            End If
            Return row
        End Function

        Public Sub QuestionsRender(ByVal dto As dtoExportQuestionnaire, ByVal questions As List(Of Domanda))
            For Each oDom As Domanda In questions
                Select Case oDom.tipo
                    Case Domanda.TipoDomanda.DropDown
                        ConvertDropDownToString(oDom, dto)
                    Case Domanda.TipoDomanda.Multipla
                        ConvertMultiplaToString(oDom, dto)
                    Case Domanda.TipoDomanda.Numerica
                        ConvertNumericaToString(oDom, dto)
                    Case Domanda.TipoDomanda.Rating
                        ConvertRatingToString(oDom, dto)
                    Case Domanda.TipoDomanda.RatingStars
                        ConvertRatingToString(oDom, dto)
                    Case Domanda.TipoDomanda.TestoLibero
                        ConvertTestoLiberoToString(oDom, dto)
                End Select
            Next
        End Sub

        Private Sub ConvertDropDownToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
            ' dto.Cells.Add(PageIdentifier & oDom.numeroPagina & QuestionIdentifier & oDom.numero)
            dto.Cells.Add(CellTitleIdQuestion & "_" & oDom.numero)
            dto.Cells.Add(GetCellIdentifier(oDom))
            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim numeroOpzList As List(Of Integer) = (From oRisp In oDom.risposteDomanda Where oRisp.idRispostaQuestionario = idAnswer Select oRisp.numeroOpzione).ToList
                row.Items.Add(oDom.id)
                If numeroOpzList.Count = 0 Then
                    row.Items.Add(NullItem)
                Else
                    row.Items.Add(numeroOpzList(0).ToString)
                End If
            Next
        End Sub
        Private Sub ConvertDropDownToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, answers As Dictionary(Of Long, QuestionnaireAnswer))
            dto.Cells.Add(CellTitleIdQuestion)
            dto.Cells.Add(GetCellIdentifier(oDom, libraryNumber))

            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim answer As QuestionnaireAnswer = answers(row.IdAnswer)

                row.Items.Add(oDom.id)
                If Not IsNothing(answer) Then
                    Dim idQuestion As Integer = oDom.id
                    '   Dim question As QuestionAnswer = (From q In answer.Answers Where q.IdQuestion = idQuestion Select q).FirstOrDefault()
                    If (From q In answer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
                        Dim numeroOpzList As List(Of Integer) = (From a In answer.Answers Where a.IdQuestion = idQuestion Select a.OptionNumber).ToList
                        If numeroOpzList.Count = 0 Then
                            row.Items.Add(NullItem)
                        Else
                            row.Items.Add(numeroOpzList(0).ToString)
                        End If
                    Else
                        row.Items.Add(QuestionNotUsedIdentifier)
                    End If
                Else
                    row.Items.Add(QuestionNotUsedIdentifier)
                End If
            Next
        End Sub
        Private Sub ConvertMultiplaToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
            Dim firstRun As Boolean = True
            Dim nOpzioni As Integer = oDom.domandaMultiplaOpzioni.Count - 1

            dto.Cells.Add(CellTitleIdQuestion & "_" & oDom.numero)

            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Order By oRisp.numeroOpzione Where oRisp.idRispostaQuestionario = idAnswer Select oRisp.numeroOpzione, oRisp.testoOpzione).ToList
                row.Items.Add(oDom.id)
                Dim listIndex As Integer = 0
                For i As Integer = 0 To nOpzioni
                    If firstRun Then
                        dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1))
                    End If

                    If listIndex < numeroOpzList.count AndAlso numeroOpzList(listIndex).numeroOpzione = i + 1 Then
                        row.Items.Add("1")

                        If oDom.domandaMultiplaOpzioni(i).isAltro Then
                            row.Items.Add(numeroOpzList(listIndex).testoOpzione)
                            If firstRun Then
                                dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1) & FreeTextIdentifier)
                            End If
                        End If
                        listIndex += 1
                    Else
                        row.Items.Add("0")
                        If oDom.domandaMultiplaOpzioni(i).isAltro Then
                            row.Items.Add(NullItem)
                            If firstRun Then
                                dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1) & FreeTextIdentifier)
                            End If
                        End If
                    End If
                Next
                firstRun = False
            Next
        End Sub
        Private Sub ConvertMultiplaToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, answers As Dictionary(Of Long, QuestionnaireAnswer))
            Dim firstRun As Boolean = True
            Dim nOpzioni As Integer = oDom.domandaMultiplaOpzioni.Count - 1

            dto.Cells.Add(CellTitleIdQuestion)
            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim qAnswer As QuestionnaireAnswer = answers(row.IdAnswer)
                Dim idQuestion As Integer = oDom.id

                row.Items.Add(idQuestion.ToString)
                If Not (From q In qAnswer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
                    Dim listIndex As Integer = 0
                    For i As Integer = 0 To nOpzioni
                        If firstRun Then
                            dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber))
                        End If
                        row.Items.Add(QuestionNotUsedIdentifier)
                    Next
                Else
                    Dim numeroOpzList = (From a In qAnswer.Answers Where a.IdQuestion = idQuestion Order By a.OptionNumber Select OptionNumber = a.OptionNumber, OptionText = a.OptionText).ToList

                    Dim listIndex As Integer = 0
                    For i As Integer = 0 To nOpzioni
                        If firstRun Then
                            dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber))
                        End If

                        If listIndex < numeroOpzList.count AndAlso numeroOpzList(listIndex).OptionNumber = i + 1 Then
                            row.Items.Add("1")

                            If oDom.domandaMultiplaOpzioni(i).isAltro Then
                                row.Items.Add(numeroOpzList(listIndex).OptionText)
                                If firstRun Then
                                    dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber).ToString & FreeTextIdentifier)
                                End If
                            End If
                            listIndex += 1
                        Else
                            row.Items.Add("0")
                            If oDom.domandaMultiplaOpzioni(i).isAltro Then
                                row.Items.Add(NullItem)
                                If firstRun Then
                                    dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber).ToString & FreeTextIdentifier)
                                End If
                            End If
                        End If
                    Next
                End If


                firstRun = False
            Next
        End Sub
        Private Sub ConvertNumericaToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
            Dim nOpzioni As Integer = oDom.opzioniNumerica.Count - 1

            dto.Cells.Add(CellTitleIdQuestion & "_" & oDom.numero)
            For i As Integer = 0 To nOpzioni
                dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1))
            Next

            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Where oRisp.idRispostaQuestionario = idAnswer Order By oRisp.numeroOpzione Select oRisp.valore, oRisp.numeroOpzione).ToList
                row.Items.Add(oDom.id)
                For i As Integer = 0 To nOpzioni
                    If IsNothing(numeroOpzList) OrElse numeroOpzList.Count = 0 Then
                        '"##############################"
                        row.Items.Add(NullItem)
                    Else
                        If Not numeroOpzList(i).valore = Integer.MinValue Then 'valore di default quando la risposta non e' stata inserita
                            row.Items.Add(numeroOpzList(i).valore.ToString)
                        Else
                            row.Items.Add(NullItem)
                        End If
                    End If
                Next
            Next
        End Sub
        Private Sub ConvertNumericaToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, answers As Dictionary(Of Long, QuestionnaireAnswer))
            Dim nOpzioni As Integer = oDom.opzioniNumerica.Count - 1
            dto.Cells.Add(CellTitleIdQuestion)
            For i As Integer = 0 To nOpzioni
                dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber))
            Next

            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim qAnswer As QuestionnaireAnswer = answers(row.IdAnswer)
                row.Items.Add(oDom.id)
                If Not IsNothing(qAnswer) Then
                    Dim idQuestion As Integer = oDom.id

                    Dim numeroOpzList = Nothing
                    If (From q In qAnswer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
                        numeroOpzList = (From a In qAnswer.Answers Where a.IdQuestion = idQuestion Order By a.OptionNumber Select Value = a.Value, OptionNumber = a.OptionNumber).ToList
                    End If

                    For i As Integer = 0 To nOpzioni
                        If Not (From q In qAnswer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
                            row.Items.Add(QuestionNotUsedIdentifier)
                        ElseIf IsNothing(numeroOpzList) OrElse numeroOpzList.Count = 0 Then
                            row.Items.Add(NullItem)
                        Else
                            If Not numeroOpzList(i).Value = Integer.MinValue.ToString Then 'valore di default quando la risposta non e' stata inserita
                                row.Items.Add(numeroOpzList(i).Value.ToString)
                            Else
                                row.Items.Add(NullItem)
                            End If
                        End If
                    Next
                Else
                    For i As Integer = 0 To nOpzioni
                        row.Items.Add(QuestionNotUsedIdentifier)
                    Next
                End If
            Next
        End Sub
        Private Sub ConvertTestoLiberoToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
            Dim nOpzioni As Integer = oDom.opzioniTestoLibero.Count - 1
            dto.Cells.Add(CellTitleIdQuestion & "_" & oDom.numero)
            For i As Integer = 0 To nOpzioni
                dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1) & FreeTextIdentifier)
            Next
            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Where oRisp.idRispostaQuestionario = idAnswer Order By oRisp.numeroOpzione Select oRisp.testoOpzione, oRisp.numeroOpzione).ToList
                row.Items.Add(oDom.id)
                For i As Integer = 0 To nOpzioni
                    If i < numeroOpzList.count AndAlso (numeroOpzList(i).numeroOpzione = i + 1 Or (oDom.opzioniTestoLibero.Count = 1 AndAlso numeroOpzList(i).numeroOpzione = 0)) Then
                        'quando ho una TL con una sola opzione, l'indice dell'opzione parte da 0. verificare.
                        row.Items.Add(numeroOpzList(i).testoOpzione)
                    Else
                        row.Items.Add(NullItem)
                    End If
                Next
            Next
        End Sub
        Private Sub ConvertTestoLiberoToString(ByRef oDom As Domanda, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, answers As Dictionary(Of Long, QuestionnaireAnswer))
            Dim nOpzioni As Integer = oDom.opzioniTestoLibero.Count - 1

            dto.Cells.Add(CellTitleIdQuestion)
            For i As Integer = 0 To nOpzioni
                dto.Cells.Add(GetCellOptionIdentifier(oDom, i + 1, libraryNumber) & FreeTextIdentifier)
            Next
            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim qAnswer As QuestionnaireAnswer = answers(row.IdAnswer)
                row.Items.Add(oDom.id)
                If Not IsNothing(qAnswer) Then
                    Dim idQuestion As Integer = CLng(oDom.id)
                    If Not (From q In qAnswer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
                        For i As Integer = 0 To nOpzioni
                            row.Items.Add(QuestionNotUsedIdentifier)
                        Next
                    Else
                        Dim numeroOpzList = (From a In qAnswer.Answers Where a.IdQuestion = idQuestion Order By a.OptionNumber Select OptionText = a.OptionText, OptionNumber = a.OptionNumber).ToList
                        For i As Integer = 0 To nOpzioni
                            If i < numeroOpzList.count AndAlso (numeroOpzList(i).OptionNumber = i + 1 Or (oDom.opzioniTestoLibero.Count = 1 AndAlso numeroOpzList(i).OptionNumber = 0)) Then
                                'quando ho una TL con una sola opzione, l'indice dell'opzione parte da 0. verificare.
                                row.Items.Add(numeroOpzList(i).OptionText)
                            Else
                                row.Items.Add(NullItem)
                            End If
                        Next
                    End If
                Else
                    For i As Integer = 0 To nOpzioni
                        row.Items.Add(QuestionNotUsedIdentifier)
                    Next
                End If
            Next
        End Sub

        Private Sub ConvertRatingToString(ByVal oDom As Domanda, ByRef dto As dtoExportQuestionnaire)
            Dim firstRun As Boolean = True
            Dim nOpzioni As Integer = oDom.domandaRating.opzioniRating.Count - 1

            dto.Cells.Add(CellTitleIdQuestion & "_" & oDom.numero)
            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer

                row.Items.Add(oDom.id)
                For j As Integer = 0 To nOpzioni
                    Dim isSelectedRow As Boolean = False
                    Dim numeroOpzList = (From oRisp In oDom.risposteDomanda Order By oRisp.idDomandaOpzione Where oRisp.idRispostaQuestionario = idAnswer And oRisp.idDomandaOpzione = oDom.domandaRating.opzioniRating(j).id Select oRisp.valore, oRisp.testoOpzione).ToList
                    For i As Integer = 0 To oDom.domandaRating.numeroRating
                        If numeroOpzList.count > 0 AndAlso numeroOpzList(0).valore = i + 1 Then
                            row.Items.Add("1")
                            isSelectedRow = True
                        Else
                            row.Items.Add("0")
                        End If
                        If firstRun Then
                            dto.Cells.Add(GetCellOptionIdentifier(oDom, j + 1) & "," & (i + 1).ToString)
                        End If
                    Next
                    If oDom.domandaRating.opzioniRating(j).isAltro Then
                        If isSelectedRow Then
                            row.Items.Add(numeroOpzList(0).testoOpzione)
                        Else
                            row.Items.Add(NullItem)
                        End If
                        If firstRun Then
                            dto.Cells.Add(GetCellOptionIdentifier(oDom, j + 1) & FreeTextIdentifier)
                        End If
                    End If
                Next
                firstRun = False
            Next
        End Sub
        Private Sub ConvertRatingToString(ByVal oDom As Domanda, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, answers As Dictionary(Of Long, QuestionnaireAnswer))
            Dim firstRun As Boolean = True
            Dim nOpzioni As Integer = oDom.domandaRating.opzioniRating.Count - 1

            dto.Cells.Add(CellTitleIdQuestion)
            For Each row As dtoExportRow In dto.Rows
                Dim idAnswer As Integer = row.IdAnswer
                Dim answer As QuestionnaireAnswer = answers(row.IdAnswer)
                row.Items.Add(oDom.id)
                If Not IsNothing(answer) Then
                    Dim idQuestion As Integer = oDom.id
                    'Dim question As Domanda = (From oRisp In quest.domande Where oRisp.id = idQuestion Select oRisp).FirstOrDefault()


                    If Not (From q In answer.Answers Where q.IdQuestion = idQuestion Select q).Any() Then
                        SetNotUsedRating(row, dto, libraryNumber, oDom, nOpzioni, firstRun)
                    Else
                        For j As Integer = 0 To nOpzioni
                            Dim isSelectedRow As Boolean = False
                            Dim index As Integer = j
                            Dim numeroOpzList = (From a In answer.Answers Where a.IdQuestion = idQuestion Order By a.IdQuestionOption Where a.IdQuestionOption = oDom.domandaRating.opzioniRating(index).id Select Value = a.Value, OptionText = a.OptionText).ToList
                            For i As Integer = 0 To oDom.domandaRating.numeroRating
                                If numeroOpzList.count > 0 AndAlso numeroOpzList(0).Value = i + 1 Then
                                    row.Items.Add("1")
                                    isSelectedRow = True
                                Else
                                    row.Items.Add("0")
                                End If
                                If firstRun Then
                                    dto.Cells.Add(GetCellOptionIdentifier(oDom, j + 1, libraryNumber) & "-" & (i + 1).ToString)
                                End If
                            Next
                            If oDom.domandaRating.opzioniRating(j).isAltro Then
                                If isSelectedRow Then
                                    row.Items.Add(numeroOpzList(0).OptionText)
                                Else
                                    row.Items.Add(NullItem)
                                End If
                                If firstRun Then
                                    dto.Cells.Add(GetCellOptionIdentifier(oDom, j + 1, libraryNumber) & FreeTextIdentifier)
                                End If
                            End If
                        Next
                    End If
                Else
                    SetNotUsedRating(row, dto, libraryNumber, oDom, nOpzioni, firstRun)
                End If

                firstRun = False
            Next
        End Sub

        Private Sub SetNotUsedRating(row As dtoExportRow, ByRef dto As dtoExportQuestionnaire, libraryNumber As Integer, ByVal oDom As Domanda, optionsNumber As Integer, firstRun As Boolean)
            For j As Integer = 0 To optionsNumber
                For i As Integer = 0 To oDom.domandaRating.numeroRating
                    row.Items.Add(QuestionNotUsedIdentifier)
                    If firstRun Then
                        dto.Cells.Add(LibraryIdentifier & libraryNumber & QuestionIdentifier & oDom.numero & OptionIdentifier & (j + 1).ToString & "-" & (i + 1).ToString)
                    End If
                Next
                If oDom.domandaRating.opzioniRating(j).isAltro Then
                    row.Items.Add(QuestionNotUsedIdentifier)
                    If firstRun Then
                        dto.Cells.Add(LibraryIdentifier & oDom.numeroPagina & QuestionIdentifier & oDom.numero & OptionIdentifier & (j + 1).ToString & FreeTextIdentifier)
                    End If
                End If
            Next
        End Sub


        Private Function GetCellIdentifier(ByRef oDom As Domanda, Optional ByVal libraryNumber As Integer = -200) As String
            If libraryNumber <> -200 Then
                Return LibraryIdentifier & libraryNumber & IIf(oDom.numero > 0, QuestionIdentifier & oDom.numero, DeletedQuestionIdentifier & oDom.VirtualNumber)
            Else
                Return PageIdentifier & oDom.numeroPagina & IIf(oDom.numero > 0, QuestionIdentifier & oDom.numero, DeletedQuestionIdentifier & oDom.VirtualNumber)
            End If
        End Function

        Private Function GetCellOptionIdentifier(ByRef oDom As Domanda, ByVal optionNumber As Integer, Optional ByVal libraryNumber As Integer = -200) As String
            If libraryNumber <> -200 Then
                Return LibraryIdentifier & libraryNumber & IIf(oDom.numero > 0, QuestionIdentifier & oDom.numero, DeletedQuestionIdentifier & oDom.VirtualNumber) & OptionIdentifier & optionNumber.ToString
            Else
                Return PageIdentifier & oDom.numeroPagina & IIf(oDom.numero > 0, QuestionIdentifier & oDom.numero, DeletedQuestionIdentifier & oDom.VirtualNumber) & OptionIdentifier & optionNumber.ToString
            End If
        End Function
#End Region


    End Class
End Namespace