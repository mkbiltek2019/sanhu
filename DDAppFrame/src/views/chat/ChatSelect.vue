<template>
  <div>
    <van-button @click="selectChat">选择群</van-button>
  </div>
</template>

<script>
import * as dd from 'dingtalk-jsapi'
import appConfig from '@/config/app.config'
import { ddAlert } from '../../service/ddJsApi.service'
import { addChat } from '../../api/ddApi'
/**
 *  选择群，用于获取群的title和chatId,用于后台定时发送群消息
 */
export default {
  name: 'ChatSelect',
  components: {

  },
  props: {

  },
  data () {
    return {

    }
  },
  created () {

  },
  methods: {
    selectChat () {
      var that = this
      dd.biz.chat.chooseConversationByCorpId({
        corpId: appConfig.corpId, // 企业id,必须是用户所属的企业的corpid
        isAllowCreateGroup: false,
        filterNotOwnerGroup: false,
        onSuccess: function (res) {
          addChat(res.chatId, res.title).then((r) => {
            var msg = '发送群信息：' + res.title + '|' + res.chatId + '成功'
            that.$toast.success(msg)
          })
          // onSuccess将在选择结束之后调用
          /* {
            chatId: 'xxxx',
            title:'xxx'
        } */
        },
        onFail: function (err) {
          ddAlert(err)
        }
      })
    }
  }
}
</script>

<style lang="less" scoped>

</style>
