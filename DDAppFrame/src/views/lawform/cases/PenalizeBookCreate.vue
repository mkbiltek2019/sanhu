<template>
  <div class="form_wapper">
    <van-cell-group title="任务信息" v-if="taskInfo">
      <van-cell title="任务标题" :value="taskInfo.TaskTitle"></van-cell>
      <van-cell title="任务内容" :value="taskInfo.TaskContent"></van-cell>
      <van-cell title="任务派发时间" :value="taskInfo.InitiationTime"></van-cell>
      <van-cell title="期望完成时间" :value="taskInfo.ExpectedCompletionTime"></van-cell>
    </van-cell-group>
    <van-cell-group v-else>
      <van-field
        v-model="caseInfo.CaseNumber"
        label="案件"
        placeholder="请选择案件"
        :readonly="true"
        clickable
        @click="handleShowSelectCase"
      >
        <van-icon name="arrow" color="#1989fa" slot="right-icon" size="25" />
      </van-field>
    </van-cell-group>
    <van-cell-group v-if="caseInfo.CauseOfAction" title="案件信息">
      <van-form @submit="onSubmit" @failed="onFailed">
        <van-cell title="案件类型" :value="caseInfo.CaseType"></van-cell>
        <van-cell title="案由" :value="caseInfo.CauseOfAction"></van-cell>
        <party-info :initData="LawParties" ref="party"></party-info>
        <van-field
          name="Illegalfacts"
          v-model="penalizeBook.Illegalfacts"
          rows="2"
          autosize
          label="违法事实"
          type="textarea"
          maxlength="200"
          placeholder="请输入违法事实"
          show-word-limit
          required
          :rules="requiredRule"
        />
        <item-group title="证据附件">
          <s-upload
            ref="myupload"
            :accept="accept"
            :sync2Dingding="true"
            :initResult="Attachment"
          >
          </s-upload>
        </item-group>
        <van-field
          v-model="illegalbasis"
          label="违法依据"
          placeholder="请选择违法依据"
          rows="4"
          autosize
          type="textarea"
          maxlength="200"
          required
          :rules="requiredRule"
        >
          <van-icon name="arrow" color="#1989fa" slot="right-icon" @click="handleShowSelectLaw('illegalbasis')" size="25" />
        </van-field>
        <van-field
          v-model="punishmentbasis"
          label="处罚依据"
          placeholder="请选择处罚依据"
          rows="4"
          autosize
          type="textarea"
          maxlength="200"
          required
          :rules="requiredRule"
        >
          <van-icon name="arrow" color="#1989fa" slot="right-icon" @click="handleShowSelectLaw('punishmentbasis')" size="25" />
        </van-field>
        <penalty-decision ref="penaltyDecision"></penalty-decision>
        <van-field
          v-model="penalizeBook.CoOrganizer"
          label="协办人"
          placeholder="请选择协办人"
          :readonly="true"
          @click="handleSelecOrganiser"
        >
          <van-icon name="arrow" color="#1989fa" slot="right-icon" size="30" />
        </van-field>

        <div class="operate-area single-save">
          <van-button type="info" :loading="loading" size="large" native-type="submit">确定</van-button>
        </div>
      </van-form>
    </van-cell-group>
    <law-list-select :showPopup="showLaw" @onClosePopup="onCloseLaw" @onPopupConfirm="onLawConfirm" v-if="showLaw"></law-list-select>
    <case-list-select :showPopup="showPopup" @onClosePopup="onCloseCase" @onPopupConfirm="onCaseConfirm"></case-list-select>
  </div>
</template>

<script>
import CaseListSelect from '../../../components/business/CaseListSelect'
import PartyInfo from '../../../components/business/PartyInfo'
import PenaltyDecision from '../../../components/business/PenaltyDecision'
import ItemGroup from '../../../components/tools/ItemGroup'
import SUpload from '../../../components/file/StandardUploadFile'
import LawListSelect from '../../../components/business/LawListSelect'
import { ddcomplexPicker } from '../../../service/ddJsApi.service'
import { isEmpty, isNotEmpty } from '../../../utils/util'
import { getDetaildata, getFormsDetailByEventInfoId } from '../../../api/regulatoryApi'
import { AcceptImageAll } from '../../../utils/helper/accept.helper'
/**
 * 当场处罚决定书
 */
export default {
  name: 'PenalizeBookCreate',
  components: {
    CaseListSelect,
    PartyInfo,
    ItemGroup,
    SUpload,
    PenaltyDecision,
    LawListSelect
  },
  data () {
    this.requiredRule = [
      { required: true, message: ' ' }
    ]
    return {
      accept: AcceptImageAll,
      loading: false,
      showPopup: false,
      showLaw: false,
      selectLawType: null,
      taskInfo: null,
      caseInfo: {
      },
      penalizeBook: {
        Illegalfacts: '',
        CoOrganizer: '',
        CoOrganizerId: ''
      },
      LawParties: [],
      Attachment: [],
      illegalbasis: '',
      punishmentbasis: ''
    }
  },
  created () {
    this.init()
  },
  methods: {
    init () {
      const queryParam = this.$route.query
      const taskId = queryParam.taskid
      if (isNotEmpty(taskId)) {
        this.loadTaskInfo(taskId)
      }
    },
    loadTaskInfo (taskId) {
      getDetaildata('work_task', taskId).then(res => {
        this.taskInfo = res
        this.loadCaseInfo(res.CaseID)
        this.loadEventCheck(res.EventInfoId)
      })
    },
    loadEventCheck (EventInfoId) {
      getFormsDetailByEventInfoId(EventInfoId, 'task_survey').then((res) => {
        if (res) {
          this.Attachment = res.attachment
        }
      })
    },
    loadCaseInfo (caseId) {
      getFormsDetailByEventInfoId(null, 'case_Info', caseId).then((res) => {
        if (res) {
          this.caseInfo = {
            ...res.MainForm
          }
          this.LawParties = res.law_party
          console.log('LawParties', res.law_party)
        }
      })
    },
    handleShowSelectCase () {
      this.showPopup = true
    },
    onCloseCase () {
      this.showPopup = false
    },
    onCaseConfirm (caseInfo) {
      this.caseInfo = caseInfo
      this.loadCaseInfo(caseInfo.ID)
      this.loadEventCheck(caseInfo.EventInfoId)
      this.showPopup = false
    },
    handleShowSelectLaw (selectLawType) {
      this.selectLawType = selectLawType
      this.showLaw = true
    },
    onCloseLaw () {
      this.selectLawType = null
      this.showLaw = false
    },
    onLawConfirm (law) {
      if (this.selectLawType === 'illegalbasis') {
        this.illegalbasis += law.lawRuleName + law.lawRuleContent
      }
      if (this.selectLawType === 'punishmentbasis') {
        this.punishmentbasis = law.lawRuleName + law.lawRuleContent
      }
      this.showLaw = false
    },
    handleSelecOrganiser () {
      ddcomplexPicker().then((res) => {
        var organiserTemp = {
          name: res.users[0].name,
          id: res.users[0].emplId
        }
        this.penalizeBook.CoOrganizer = organiserTemp.name
        this.penalizeBook.CoOrganizerId = organiserTemp.id
      })
    },
    onSubmit (values) {
      console.log('submit', values)
      if (isEmpty(this.caseInfo.CauseOfAction)) {
        this.$toast('请选择案件')
        return
      }
      const LawParties = this.$refs.party.getResult()
      const decisions = this.$refs.penaltyDecision.getResult()
      var attachments = this.$refs.myupload.getUploadResult()
      var forms = {
        taskInfo: this.taskInfo,
        ...this.penalizeBook,
        ...values,
        caseInfo: {
          ...this.caseInfo
        },
        illegalbasis: this.illegalbasis,
        punishmentbasis: this.punishmentbasis,
        LawParties: LawParties,
        decisions: decisions,
        attachments: attachments
      }
      this.$router.push({ name: 'penalizeBookPreview', params: { forms: forms } })
    },
    onFailed (errorInfo) {
      console.log('failed', errorInfo)
    }
  }
}
</script>

<style lang="less" scoped>

</style>
